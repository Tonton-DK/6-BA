using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class ServiceCheckModel : PageModel
{
    private readonly IUserService _userService;
    private readonly IJobService _jobService;
    private readonly IOfferService _offerService;
    private readonly IContractService _contractService;
    private readonly IReviewService _reviewService;

    public Dictionary<Type, bool> ServiceStatus { get; private set; }

    public ServiceCheckModel(ILogger<IndexModel> logger, 
        IUserService userService, 
        IJobService jobService, 
        IOfferService offerService, 
        IContractService contractService, 
        IReviewService reviewService)
    {
        _userService = userService;
        _jobService = jobService;
        _offerService = offerService;
        _contractService = contractService;
        _reviewService = reviewService;
        ServiceStatus = new Dictionary<Type, bool>();
    }

    public void OnGet()
    {
        ServiceStatus.Add(_userService.GetType(), _userService.Get());
        ServiceStatus.Add(_jobService.GetType(), _jobService.Get());
        ServiceStatus.Add(_offerService.GetType(), _offerService.Get());
        ServiceStatus.Add(_contractService.GetType(), _contractService.Get());
        ServiceStatus.Add(_reviewService.GetType(), _reviewService.Get());
    }
}