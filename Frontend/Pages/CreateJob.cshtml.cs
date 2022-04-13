using Microsoft.AspNetCore.Mvc.RazorPages;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;

namespace Frontend.Pages;

public class CreateJobModel : PageModel
{
    private readonly ILogger<CreateJobModel> _logger;
    
    private readonly IJobService _jobService;
    
    public Dictionary<Type, bool> ServiceStatus { get; private set; }
    
    public Job Job { get; private set; }
    
    public CreateJobModel(ILogger<CreateJobModel> logger,
        IJobService jobService)
    {
        _logger = logger;
        _jobService = jobService;
        ServiceStatus = new Dictionary<Type, bool>();
    }
    public void OnGet()
    {
        
    }
    
}