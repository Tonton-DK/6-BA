using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

public class ViewUserModel : LayoutModel
{
    private readonly ILogger<ViewUserModel> _logger;
    
    private readonly IUserService _userService;
    private readonly IJobService _jobService;
    private readonly IContractService _contractService;
    private readonly IReviewService _reviewService;

    public Dictionary<Type, bool> ServiceStatus { get; private set; }

    public User? Client { get; private set; }
    public IEnumerable<Job> Jobs { get; private set; }
    public IEnumerable<Contract> OpenContracts { get; private set; }
    public IEnumerable<Contract> ClosedContracts { get; private set; }
    public IEnumerable<Review> ReviewsAsClient { get; private set; }
    public IEnumerable<Review> ReviewsAsProvider { get; private set; }
    
    public ViewUserModel(ILogger<ViewUserModel> logger,
        IJobService jobService,
        IUserService userService,
        IContractService contractService,
        IReviewService reviewService)
    {
        _logger = logger;
        _jobService = jobService;
        _userService = userService;
        _contractService = contractService;
        _reviewService = reviewService;
        ServiceStatus = new Dictionary<Type, bool>();
    }
    
    public IActionResult OnGet()
    {
        Instantiate();
        
        OpenContracts = new List<Contract>();
        Jobs = new List<Job>();
        ReviewsAsClient = new List<Review>();
        ReviewsAsProvider = new List<Review>();
        var contracts = new List<Contract>();
        contracts.Add(new Contract(Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, DateTime.Now,State.Cancelled));
        contracts.Add(new Contract(Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, Guid.Empty, DateTime.Now,State.Open));
        ClosedContracts = contracts;
        
        return Page();
    }
}