using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Frontend.Pages;

public class IndexModel : LayoutModel
{
    private readonly ILogger<IndexModel> _logger;

    private readonly IJobService _jobService;

    public SelectList Categories { get; set; }
    
    [BindProperty]
    public Guid CategoryId { get; set; }

    public IndexModel(ILogger<IndexModel> logger,
        IJobService jobService)
    {
        _logger = logger;
        _jobService = jobService;
    }

    public IActionResult OnGet()
    {
        Instantiate();
        Categories = new SelectList(_jobService.ListCategories(), nameof(Category.Id), nameof(Category.Name));
        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        var id = CategoryId;
        return RedirectToPage("CreateTask", new { categoryId = CategoryId });
    }
}