using GroupProject.DataAccess;
using GroupProject.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroupProject.Models;
using System.Diagnostics.CodeAnalysis;

namespace GroupProject.Controllers;

[ApiController]
[Route("api")]
public class APIController : Controller
{
    private readonly ILogger<APIController> _logger;
    private readonly DataService _dataService;

    public APIController(ILogger<APIController> logger, DataContext dataContext)
    {
        _logger = logger;
        _dataService = new DataService(dataContext);
    }

    [HttpGet("getjobcode")]
    public ActionResult<JobCode> actionResult(int id)
    {
        JobCode jobCode = _dataService.GetJobCode(id);
        if (jobCode == null)
        {
            return NotFound();
        }

        return jobCode;
    }
}