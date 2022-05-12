using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Pages;

[IgnoreAntiforgeryToken]
public class CreateJobModel : LayoutModel
{
    private readonly ILogger<CreateJobModel> _logger;
    
    private readonly IJobService _jobService;
    
    [BindProperty]
    public Job Job { get; set; }
    public SelectList Categories { get; set; }

    public CreateJobModel(ILogger<CreateJobModel> logger,
        IJobService jobService)
    {
        _logger = logger;
        _jobService = jobService;
        Job = new Job();
    }

    public IActionResult OnGet(Guid? categoryId = null)
    {
        if (!SessionLoggedIn) return RedirectToPage("Login");
        
        Categories = new SelectList(_jobService.ListCategories(), nameof(Category.Id), nameof(Category.Name));
        if (categoryId != null)
        {
            Job.Category.Id = categoryId.Value;
        }
        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        if (!SessionLoggedIn) return RedirectToPage("Login");
        
        var clientId = SessionId;
        Job.ClientId = clientId;
        var job = _jobService.CreateJob(Job);
        if (job != null)
        {
            return RedirectToPage("ViewTask", new {jobId = job.Id});
        }
        return OnGet();
    }
}