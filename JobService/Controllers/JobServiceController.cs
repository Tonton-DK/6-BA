using ClassLibrary.Classes;
using JobService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JobService.Controllers;

[ApiController]
[Route("[controller]")]
public class JobServiceController : ControllerBase//, IJobService
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
        var jobs = new List<Job>();
        return jobs.ToArray();
    }
    
    [HttpGet("ListCategories")]
    public IEnumerable<Category> ListCategories()
    {
        return _dataProvider.ListCategories();
    }
    
    [HttpPost("CreateJob")]
    public Job? CreateJob(Job job)
    {
        return null;
    }
    
    [HttpGet("GetJobById/{id}")]
    public Job? GetJobById(Guid id)
    {
        return null;
    }

    [HttpGet("ListJobs")]
    public IEnumerable<Job> ListJobs(Filter filter)
    {
        return null;
    }
    
    [HttpPut("UpdateJob")]
    public Job? UpdateJob(Job job)
    {
        return null;
    }

    [HttpDelete("DeleteJobById/{id}")]
    public bool DeleteJobById(Guid id)
    {
        return false;
    }
}
