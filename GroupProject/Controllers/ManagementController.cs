using GroupProject.DataAccess;
using GroupProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Drawing.Imaging;
using System.Security.Claims;

namespace GroupProject.Controllers
{
    [Route("management")]
    public class ManagementController : Controller
    {
        private readonly ILogger<ManagementController> _logger;
        private readonly DataService _dataService;

        public ManagementController(ILogger<ManagementController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataService = new DataService(dataContext);
        }

        [Authorize(Roles = "Admin, Management")]
        public IActionResult Index()
        {
            List<Employee> employees = User.IsInRole("Admin") ? employees = _dataService.GetEmployees() : employees = _dataService.GetEmployees()
                .Where(e => e.Manager.ToString() == User.FindFirst(ClaimTypes.NameIdentifier)?.Value).ToList();
            return View(employees);
        }

        [Route("timecard/{id:int}")]
        [Authorize(Roles = "Admin, Management")]
        public IActionResult Timecard(int id, string payPeriod)
        {
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

        [HttpGet("EditBreaks")]
        [Authorize(Roles = "Admin, Management")]
        public ActionResult EditBreaks(string hid, string eid)
        {
            int hourId = hid == "null" ? 0 : int.Parse(hid);
            Hour hour = _dataService.GetHour(hourId);
            if (hour == null)
            {
                return null;
            }

            var breaks = _dataService.GetBreaks();
            breaks = breaks.Where(b => b.HourId == hourId).ToList();
            ViewBag.Hour = hour;
            ViewBag.EID = int.Parse(eid);
            return View(breaks);
        }

        [HttpGet("EditHours")]
        [Authorize(Roles = "Admin, Management")]
        public ActionResult EditHours(string hid, string eid)
        {
            int hourId = hid == "null" ? 0 : int.Parse(hid);
            int employeeId = string.IsNullOrEmpty(eid) ? 0 : int.Parse(eid);
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

        [HttpPost("updatehours")]
        [Authorize(Roles ="Admin, Management")]
        public void UpdateHours(string eid, string hid, string StartDate, string StartTime, string ClockedIn, string EndDate, string EndTime, string JobCode, string Comment)
        {
            int hourId = hid == "0" ? 0 : int.Parse(hid);
            int employeeId = string.IsNullOrEmpty(eid) ? 0 : int.Parse(eid);
            Hour hour = _dataService.GetHour(hourId);
            if (hour == null)
            {
                hour = new Hour();
            }

            Employee employee = _dataService.GetEmployee(employeeId);
            hour.EmployeeId = employeeId;
            hour.TimeIn = DateTime.Parse(StartDate).Add(TimeSpan.Parse(StartTime));
            hour.TimeOut = ClockedIn == "true" ? null : DateTime.Parse(EndDate).Add(TimeSpan.Parse(EndTime));
            EmployeeJobCode ejc = _dataService.GetEmployeeJobCode(int.Parse(JobCode));
            if (Comment != null)
            {
                if (Comment.Length > 250)
                {
                    string cutCom = Comment.Substring(0, 250);
                    Comment = cutCom;
                }
            }
            hour.Comment = Comment;
            PayPeriod payPeriod = _dataService.GetPayPeriods().Where(p => p.Start <= hour.TimeIn && (p.End >= hour.TimeOut || hour.TimeOut == null)).FirstOrDefault();
            hour.PayPeriodId = payPeriod.Id;
            hour.EmployeeId = employeeId;
            hour.EmployeeJobCodeId = ejc.Id;
            hour.PayRate = ejc.PayRate;
            hour.TimeEntered = DateTime.Now;
            hour.EmployeeApproved = false;

            if (hour.Id == 0)
            {
                _dataService.AddHour(hour);
            }
            else
            {
                _dataService.UpdateHour(hour);
            }
        }

        [HttpPost("UpdateBreaks")]
        [Authorize(Roles = "Admin, Management")]
        public void UpdateBreaks(string hid, string nbc)
        {
            int count = int.Parse(nbc);
            int hourId = hid == "null" ? 0 : int.Parse(hid);
            Hour hour = _dataService.GetHour(hourId);
            if (hour == null)
            {
                return;
            }

            var breaks = _dataService.GetBreaks();
            breaks = breaks.Where(b => b.HourId == hourId).ToList();
            foreach(Break b in breaks)
            {
                string id = Request.Headers["Row" + b.Id];
                if (id == null)
                {
                    _dataService.RemoveBreak(b);
                    continue;
                }

                b.StartTime = b.StartTime.Date.Add(TimeSpan.Parse(Request.Headers["Start" + id]));
                b.EndTime = hour.TimeIn.Date.Add(TimeSpan.Parse(Request.Headers["End" + id]));
                b.Comment = Request.Headers["Comment" + id];
                b.Paid = Request.Headers["Paid" + id] == "true";
                _dataService.UpdateBreak(b);
            }

            for (int i = 1; i < count + 1; i++)
            {
                string id = i.ToString();
                Break b = new Break();
                b.StartTime = hour.TimeIn.Date.Add(TimeSpan.Parse(Request.Headers["NewStart" + id]));
                b.EndTime = hour.TimeIn.Date.Add(TimeSpan.Parse(Request.Headers["NewEnd" + id]));
                b.Comment = Request.Headers["NewComment" + id];
                b.Paid = Request.Headers["NewPaid" + id] == "true";
                b.HourId = hour.Id;
                _dataService.AddBreak(b);
            }
        }

        [HttpPost("deletehours/{id:int}")]
        [Authorize(Roles = "Admin, Management")]
        public void DeleteHours(int id)
        {
            Hour hour = _dataService.GetHour(id);
            _dataService.RemoveHour(hour);
        }

        [HttpGet("approvetimecard")]
        [Authorize(Roles ="Admin, Management")]
        public ActionResult ApproveTimecard(string eid, string pid)
        {
            ViewBag.EID = eid;
            ViewBag.PID = pid;
            return View();
        }

        [HttpPost("approvetimecard")]
        [Authorize(Roles = "Admin, Management")]
        public void ConfirmTimecard(string eid, string pid)
        {
            int e = int.Parse(eid);
            int p = int.Parse(pid);
            foreach(var hour in _dataService.GetHours().Where(h => h.PayPeriodId == p && h.EmployeeId == e))
            {
                hour.ApprovedBy = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _dataService.ApproveHour(hour);
            }
        }
    }
}
