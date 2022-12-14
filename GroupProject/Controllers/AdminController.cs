using GroupProject.DataAccess;
using GroupProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroupProject.Models;
using System.Drawing.Drawing2D;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Authorization;

namespace GroupProject.Controllers;

[Route("admin")]
public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly DataService _dataService;
    private int id;

    public AdminController(ILogger<AdminController> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataService = new DataService(dataContext);
    }

    [Route("")]
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        return View(_dataService.GetEmployees());
    }

    [HttpGet("addemployee")]
    [Authorize(Roles = "Admin")]
    public ActionResult AddEmployee()
    {
        EmployeeViewModel employeeViewModel = new EmployeeViewModel();
        employeeViewModel.Employees = _dataService.GetEmployees();

        return View(employeeViewModel);
    }

    [HttpPost("addemployee")]
    [Authorize(Roles = "Admin")]
    public ActionResult AddEmployee(Employee employee)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        _dataService.AddEmployee(employee);

        return Redirect("/admin");
    }

    [HttpGet("editemployee/{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult EditEmployee(int id)
    {
        Employee e = _dataService.GetEmployee(id);
        e.Login = _dataService.GetLogin(id);
        EmployeeViewModel vm = new EmployeeViewModel()
        {
            Employee = e,
            JobCodes = _dataService.GetJobCodes(),
            Employees = _dataService.GetEmployees()
        };

        return View(vm);
    }

    [HttpPost("editemployee/{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult EditEmployee(EmployeeViewModel vm)
    {
        vm.Employees = _dataService.GetEmployees();

        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        if (_dataService.GetEmployees().FirstOrDefault(x => x.Id == vm.Employee.Id) == null)
        {
            return View();
        }

        _dataService.UpdateEmployee(vm.Employee);
        return Redirect("/admin");
    }

    [Route("assignjobcodes/{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult AssignJobCodes(int id)
    {
        Employee e = _dataService.GetEmployee(id);
        return View(e);
    }

    [HttpGet("addemployeejobcode")]
    [Authorize(Roles = "Admin")]
    public IActionResult AddEmployeeJobCode(int id)
    {
        Employee e = _dataService.GetEmployee(id);
        EmployeeJobCodeViewModel model = new EmployeeJobCodeViewModel()
        {
            JobCodes = _dataService.GetJobCodes(),
            EmployeeId = id
        };

        return View(model);
    }

    [HttpPost("addemployeejobcode")]
    [Authorize(Roles = "Admin")]
    public IActionResult AddEmployeeJobCode(EmployeeJobCodeViewModel e, int id)
    {
        e.JobCodes = _dataService.GetJobCodes();
        e.EmployeeJobCode.Employee = _dataService.GetEmployee(id);
        e.EmployeeJobCode.EmployeeId = e.EmployeeJobCode.Employee.Id;
        e.EmployeeId = id;

        if (!ModelState.IsValid)
        {
            return View(e);
        }
        foreach (EmployeeJobCode jc in _dataService.GetEmployee(id).EmployeeJobCodes)
        {
            if (e.EmployeeJobCode.JobCodeId == jc.JobCodeId)
            {
                Employee employee = e.EmployeeJobCode.Employee;
                JobCode j = _dataService.GetJobCode(e.EmployeeJobCode.JobCodeId);
                ViewBag.ErrorMessage = $"{employee.FirstName} {employee.LastName} already has [{j.JobTitle} - {j.Id}] listed in their Job Codes.";
                return View(e);
            }
        }

        _dataService.AddEmployeeJobCode(e.EmployeeJobCode);
        string redirect = $"/admin/assignjobcodes/{id}";

        return Redirect(redirect);
    }

    [HttpGet("editemployeejobcode/{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult EditEmployeeJobCode(int id)
    {
        EmployeeJobCode jc = _dataService.GetEmployeeJobCode(id);
        EmployeeJobCodeViewModel vm = new EmployeeJobCodeViewModel()
        {
            EmployeeJobCode = jc,
            JobCodes = _dataService.GetJobCodes()
        };

        vm.JobCode = _dataService.GetJobCode(vm.EmployeeJobCode.JobCodeId);

        return View(vm);
    }

    [HttpPost("editemployeejobcode/{id:int}")]
    [Authorize(Roles = "Admin")]
    public IActionResult EditEmployeeJobCode(EmployeeJobCodeViewModel vm, int id)
    {
        vm.JobCodes = _dataService.GetJobCodes();
        vm.EmployeeJobCode.Employee = _dataService.GetEmployee(id);

        if (id == 0)
        {
            id = _dataService.GetEmployee(_dataService.GetEmployeeJobCode(vm.EmployeeJobCode.Id).EmployeeId).Id;
        }

        vm.EmployeeId = id;

        if (!ModelState.IsValid)
        {
            vm.JobCodes = _dataService.GetJobCodes();
            vm.EmployeeJobCode.Employee = _dataService.GetEmployee(id);
            return View(vm);
        }
        if (_dataService.GetEmployeeJobCode(vm.EmployeeJobCode.Id) == null)
        {
            vm.JobCodes = _dataService.GetJobCodes();
            return View();
        }

        int rowCount = 0;

        foreach (EmployeeJobCode jc in _dataService.GetEmployee(id).EmployeeJobCodes)
        {
            if (vm.EmployeeJobCode.JobCodeId == jc.JobCodeId)
            {
                rowCount++;
            }
        }
        
        if (rowCount > 1)
            {
                vm.JobCodes = _dataService.GetJobCodes();
                vm.EmployeeJobCode.Employee = _dataService.GetEmployee(id);
                vm.EmployeeId = id;
                Employee employee = vm.EmployeeJobCode.Employee;
                JobCode j = _dataService.GetJobCode(vm.EmployeeJobCode.JobCodeId);
                ViewBag.ErrorMessage = $"{employee.FirstName} {employee.LastName} already has [{j.JobTitle} - {j.Id}] listed in their Job Codes.";
                return View(vm);
            }

        string redirect = $"/admin/assignjobcodes/{id}";

        _dataService.UpdateEmployeeJobCode(vm.EmployeeJobCode);
        return Redirect(redirect);
    }

    [HttpGet("addjobcode")]
    [Authorize(Roles = "Admin")]
    public ActionResult AddJobCode()
    {
        return View();
    }

    [HttpPost("addjobcode")]
    [Authorize(Roles = "Admin")]
    public ActionResult AddJobCode(JobCode jobcode)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        _dataService.AddJobCode(jobcode);

        return Redirect("/admin");
    }

    [HttpGet("editjobcode")]
    [Authorize(Roles = "Admin")]
    public IActionResult EditJobCode()
    {
        JobCodeViewModel jobCodeViewModel = new JobCodeViewModel();
        jobCodeViewModel.JobCodes = _dataService.GetJobCodes();
        return View(jobCodeViewModel);
    }

    [HttpPost("editjobcode")]
    [Authorize(Roles = "Admin")]
    public IActionResult EditJobCode(JobCode jobCode)
    {
        if (!ModelState.IsValid)
        {
            JobCodeViewModel jobCodeViewModel = new JobCodeViewModel();
            jobCodeViewModel.JobCodes = _dataService.GetJobCodes();

            return View(jobCodeViewModel);
        }
        if (_dataService.GetJobCodes().FirstOrDefault(x => x.Id == jobCode.Id) == null)
        {
            return View();
        }

        _dataService.EditJobCode(jobCode);
        return Redirect("/admin");
    }


    [Route("employeepreview/{id:int}")]
    [Authorize(Roles = "Admin")]
    public ActionResult EmployeePreview(int id)
    {
        var model = _dataService.GetEmployee(id);
        return View(model);
    }

}