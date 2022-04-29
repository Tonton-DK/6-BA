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
    
    public Dictionary<Type, bool> ServiceStatus { get; private set; }
    
    public Job Job { get; private set; }
    public SelectList Categories { get; set; }

    public CreateJobModel(ILogger<CreateJobModel> logger,
        IJobService jobService)
    {
        _logger = logger;
        _jobService = jobService;
        ServiceStatus = new Dictionary<Type, bool>();
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
}