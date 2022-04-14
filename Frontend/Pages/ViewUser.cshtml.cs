using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class ViewUserModel : PageModel
{
    private readonly ILogger<ViewUserModel> _logger;
    
    private readonly IUserService _userService;
    private readonly IJobService _jobService;
    private readonly IContractService _contractService;
    private readonly IReviewService _reviewService;

    public Dictionary<Type, bool> ServiceStatus { get; private set; }

    public User Client { get; private set; }
    public IEnumerable<Job> Jobs { get; private set; }
    public IEnumerable<Contract> OpenContracts { get; private set; }
    public IEnumerable<Contract> ClosedContracts { get; private set; }
    
    public ViewUserModel(ILogger<ViewUserModel> logger,
        IJobService jobService,
        IUserService userService,
        IContractService contractService)
    {
        _logger = logger;
        _jobService = jobService;
        _userService = userService;
        _contractService = contractService;
        ServiceStatus = new Dictionary<Type, bool>();
    }
    public void OnGet()
    {
        
    }
}