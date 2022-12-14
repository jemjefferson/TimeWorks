using GroupProject.DataAccess;
using GroupProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using GroupProject.Models;
using System.Xml.Linq;

namespace GroupProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DataService _dataService;

    public HomeController(ILogger<HomeController> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataService = new DataService(dataContext);
    }

    [Route("")]
    [Authorize]
    public IActionResult Index()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Employee e = _dataService.GetEmployee(userId);

        foreach (EmployeeJobCode jc in e.EmployeeJobCodes)
        {
            if (jc.Active == true)
            {
                ViewBag.ClockIn = true;
                break;
            }
            else
            {
                ViewBag.ClockIn = false;
            }
        }

        string status = "Clocked Out";
        var lastHour = _dataService.GetHours().Where(h => h.EmployeeId == e.Id && h.TimeIn < DateTime.Now.Date.AddDays(1)).OrderByDescending(h => h.TimeIn).FirstOrDefault();
        if (lastHour.TimeOut == null)
        {
            var lastBreak = _dataService.GetBreaks().Where(h => h.HourId == lastHour.Id).OrderByDescending(h => h.StartTime).FirstOrDefault();
            if (lastBreak == null || lastBreak?.EndTime != null)
            {
                status = "Clocked In";
            }
            else
            {
                status = "On Break";
            }
        }

        ViewBag.Status = status;

        return View(e);
    }

    [Route("timecard")]
    [Authorize]
    public IActionResult Timecard(string payPeriod)
    {
        int id = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        var model = _dataService.GetEmployee(id);
        model.Hours = _dataService.GetHours().Where(h => h.EmployeeId == id).ToList();
        var payPeriods = _dataService.GetPayPeriods();
        if (payPeriods.Count == 0)
        {
            DateTime start = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek);
            _dataService.AddPayPeriod(new PayPeriod()
            {
                Start = start,
                End = start.AddDays(14),
            });
        }
        var current = payPeriods.Where(p => p.Start <= DateTime.Now && p.End >= DateTime.Now).FirstOrDefault();
        if (current == null)
        {
            DateTime start = payPeriods.OrderByDescending(p => p.Start).First().End;
            current = _dataService.AddPayPeriod(new PayPeriod()
            {
                Start = start,
                End = start.AddDays(14),
            });
        }
        ViewBag.PayPeriods = payPeriods;
        ViewBag.PayPeriod = string.IsNullOrEmpty(payPeriod) ? current : _dataService.GetPayPeriod(int.Parse(payPeriod));
        return View(model);
    }

    [HttpPost("approvetimecard")]
    [Authorize]
    public void ApproveTimecard(string pid)
    {
        int e = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        int p = int.Parse(pid);
        foreach (var hour in _dataService.GetHours().Where(h => h.PayPeriodId == p && h.EmployeeId == e))
        {
            hour.EmployeeApproved = true;
            _dataService.EmployeeApproveHour(hour);
        }
    }

    [HttpGet("clock-in")]
    [Authorize]
    public IActionResult ClockIn()
    {
        string user = User.Identity.Name;
        Employee model = _dataService.GetEmployeeFromString(user);
        foreach (EmployeeJobCode jc in model.EmployeeJobCodes)
        {
            jc.JobCode = _dataService.GetJobCode(jc.JobCodeId);
        }
        return View(model);
    }

    [HttpPost("clock-in")]
    [Authorize]
    public IActionResult ClockIn(string jcid, string tz)
    {
        int id = int.Parse(jcid);
        EmployeeJobCode jc = _dataService.GetEmployeeJobCode(id);
        Hour hour = new Hour { EmployeeId = jc.EmployeeId, EmployeeJobCodeId = jc.Id, PayRate = jc.PayRate, TimeIn = DateTime.UtcNow.AddMinutes(-int.Parse(tz)), TimeOut = null, TimeEntered = DateTime.Now, EmployeeApproved = false };
        if (!ModelState.IsValid)
        {
            return View(_dataService.GetEmployee(jc.EmployeeId));
        }

        _dataService.AddHours(hour);
        return Redirect("/");
    }

    [HttpPost("clock-out")]
    [Authorize]
    public IActionResult ClockOut(string tz)
    {
        bool result = false;
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        Employee e = _dataService.GetEmployee(userId);

        if (DateTime.Now < e.Hours.OrderByDescending(h => h.TimeIn).First().TimeIn.AddMinutes(1))
        {
            return Redirect("/");
        }

        foreach (EmployeeJobCode ejc in e.EmployeeJobCodes)
        {
            if (ejc.Active == true)
            {
                result = true;
                break;
            }
        }

        if (result == false)
        {
            return Redirect("/");
        }
        else
        {
            _dataService.EndHours(e.Id, tz);
            return Redirect("/");
        }
    }

    [Authorize]
    [HttpGet("SelectBreak")]
    public ActionResult SelectBreak()
    {
        Employee e = _dataService.GetEmployee(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        return View(e);
    }

    [Authorize]
    [HttpPost("StartBreak")]
    public ActionResult StartBreak(string p, string tz)
    {
        Employee e = _dataService.GetEmployee(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        Hour hour = _dataService.GetHours().Where(h => h.EmployeeId == e.Id && h.TimeOut == null).OrderByDescending(h => h.TimeIn).FirstOrDefault();
        if (hour != null)
        {
            if (_dataService.GetBreaks().Where(b => b.EndTime == null && b.HourId == hour.Id).Count() == 0)
            {
                Break brk = new Break();
                brk.HourId = hour.Id;
                brk.StartTime = DateTime.UtcNow.AddMinutes(-int.Parse(tz));
                brk.Paid = p == "y";
                _dataService.AddBreak(brk);
            }
        }

        return RedirectToAction("index");
    }

    [Authorize]
    [HttpPost("EndBreak")]
    public ActionResult EndBreak(string tz)
    {
        Employee e = _dataService.GetEmployee(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
        Hour hour = _dataService.GetHours().Where(h => h.EmployeeId == e.Id && h.TimeOut == null).OrderByDescending(h => h.TimeIn).FirstOrDefault();
        if (hour != null)
        {
            Break brk = _dataService.GetBreaks().Where(b => b.EndTime == null && b.HourId == hour.Id).OrderByDescending(b => b.EndTime).FirstOrDefault();
            if (brk != null)
            {
                brk.EndTime = DateTime.UtcNow.AddMinutes(-int.Parse(tz));
                _dataService.UpdateBreak(brk);
            }
        }

        return RedirectToAction("index");
    }

    [HttpGet("EditHourComment")]
    [Authorize]
    public ActionResult EditHourComment(string hid)
    {
        int employeeId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        int hourId = hid == "null" ? 0 : int.Parse(hid);
        Hour hour = new Hour();
        if (hourId == 0)
        {
            hour.TimeIn = DateTime.Now.Date.AddHours(9);
            hour.TimeOut = DateTime.Now.Date.AddHours(17);
        }
        else
        {
            hour = _dataService.GetHour(hourId);
        }

        Employee employee = _dataService.GetEmployee(employeeId);
        ViewBag.Employee = employee;
        return View(hour);
    }

    [HttpPost("updatehourcomment")]
    [Authorize]
    public void UpdateHourComment(string hid, string Comment)
    {   
        if (Comment != null)
        {
            if (Comment.Length > 250)
            {
                string cutCom = Comment.Substring(0, 250);
                Comment = cutCom;
            }
        }

        int hourId = hid == "0" ? 0 : int.Parse(hid);
        _dataService.UpdateHourComment(hourId, Comment);
    }

    [HttpGet("EditBreakComment")]
    [Authorize]
    public ActionResult EditBreakComment(string bid)
    {
        int employeeId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        int breakId = bid == "null" ? 0 : int.Parse(bid);
        Break brk = _dataService.GetBreak(breakId);

        Employee employee = _dataService.GetEmployee(employeeId);
        ViewBag.Employee = employee;
        return View(brk);
    }

    [HttpPost("updatebreakcomment")]
    [Authorize]
    public void UpdateBreakComment(string bid, string Comment)
    {
        if (Comment != null)
        {
            if (Comment.Length > 250)
            {
                string cutCom = Comment.Substring(0, 250);
                Comment = cutCom;
            }
        }

        int breakId = bid == "0" ? 0 : int.Parse(bid);
        _dataService.UpdateBreakComment(breakId, Comment);
    }
}