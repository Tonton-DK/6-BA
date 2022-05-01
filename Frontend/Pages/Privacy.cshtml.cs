using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

public class PrivacyModel : LayoutModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public IActionResult OnGet()
    {
        return Page();
    }
}

