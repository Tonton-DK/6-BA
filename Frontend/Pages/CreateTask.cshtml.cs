using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Pages;

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

    public IActionResult OnGet(Guid? categoryId)
    {
        Instantiate();
        Categories = new SelectList(_jobService.ListCategories(), nameof(Category.Id), nameof(Category.Name));
        if (categoryId != null && Categories.Any(x => x.Value == categoryId.ToString()))
        {
            Categories.FirstOrDefault(x => x.Value == categoryId.ToString())!.Selected = true;
        }
        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        var clientId = new Guid(HttpContext.Session.GetString(SessionIdKey));
        Job.ClientId = clientId;
        var jobId = _jobService.CreateJob(Job);
        return RedirectToPage("ViewTask", new { jobId = jobId });
    }
}