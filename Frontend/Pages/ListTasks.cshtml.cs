using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class ListJobsModel : PageModel
{
    private readonly ILogger<ListJobsModel> _logger;
    
    private readonly IJobService _jobService;
    
    public Dictionary<Type, bool> ServiceStatus { get; private set; }

    public IEnumerable<Job> Jobs { get; private set; }
    
    public ListJobsModel(ILogger<ListJobsModel> logger,
        IJobService jobService)
    {
        _logger = logger;
        _jobService = jobService;
        ServiceStatus = new Dictionary<Type, bool>();
    }
    
    public void OnGet()
    {
        var filter = new Filter(null, null, null, "", "");
        Jobs = _jobService.ListJobs(filter);
    }
}