using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

public class ViewContractModel : LayoutModel
{
    private readonly ILogger<ViewContractModel> _logger;
    
    private readonly IJobService _jobService;
    private readonly IUserService _userService;
    private readonly IOfferService _offerService;
    
    public ViewContractModel(ILogger<ViewContractModel> logger,
        IJobService jobService,
        IUserService userService,
        IOfferService offerService)
    {
        _logger = logger;
        _jobService = jobService;
        _userService = userService;
        _offerService = offerService;
    }

    public IActionResult OnGet(Guid contractId)
    {
        Instantiate();
        return Page();
    }
}