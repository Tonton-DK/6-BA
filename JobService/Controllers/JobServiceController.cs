using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using JobService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobService.Controllers;

[ApiController]
[Route("[controller]")]
public class JobServiceController : ControllerBase, IJobService
{
    private readonly ILogger<JobServiceController> _logger;
    private readonly IDataProvider _dataProvider;

    public JobServiceController(ILogger<JobServiceController> logger, IDataProvider dataProvider)
    {
        _logger = logger;
        _dataProvider = dataProvider;
    }

    [HttpGet]
    public IEnumerable<Job> Get()
    {
        var jobs = _dataProvider.GetJobs();
        return jobs.ToArray();
    }

    [HttpGet("GetByName/{name}")]
    public IActionResult GetByName(string name)
    {
        return Ok("Name: " + name);
    }

    [HttpGet("GetById/{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok("Id: " + id);
    } 
}
