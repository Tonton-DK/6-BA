using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

public class ViewContractModel : LayoutModel
{
    private readonly ILogger<ViewContractModel> _logger;
    
    private readonly IContractService _contractService;
    private readonly IUserService _userService;
    private readonly IJobService _jobService;
    private readonly IOfferService _offerService;

    public bool ValidContract;
    public Contract Contract;
    public User Client;
    public User Provider;
    public Job Job;
    public Offer Offer;

    public ViewContractModel(ILogger<ViewContractModel> logger,
        IJobService jobService,
        IUserService userService,
        IContractService contractService,
        IOfferService offerService)
    {
        _logger = logger;
        _jobService = jobService;
        _userService = userService;
        _contractService = contractService;
        _offerService = offerService;
    }

    public IActionResult OnGet(Guid contractId)
    {
        Instantiate();
        
        Contract = _contractService.GetContractById(contractId);

        if (!SessionLoggedIn || (Contract.ClientId == contractId || Contract.ProviderId == contractId))
        {
            ViewData["Title"] = "Not allowed";
            ViewData["Message"] = "You are not allowed to view this contract.";
            ValidContract = false;
            return Page();
        }
        
        Client = _userService.GetUserById(Contract.ClientId);
        Provider = _userService.GetUserById(Contract.ProviderId);
        Job = _jobService.GetJobById(Contract.JobId);
        Offer = _offerService.GetOfferById(Contract.OfferId);

        return Page();
    }
}