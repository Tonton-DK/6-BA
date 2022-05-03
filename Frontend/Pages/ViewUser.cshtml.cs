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
    private readonly IOfferService _offerService;
    private readonly IContractService _contractService;
    private readonly IReviewService _reviewService;
    
    public User? Client { get; private set; }
    public IEnumerable<Job> Jobs { get; private set; }
    public IEnumerable<Contract> OpenContracts { get; private set; }
    public IEnumerable<Contract> ClosedContracts { get; private set; }
    public IEnumerable<Job> JobsForContracts { get; private set; }
    public IEnumerable<Offer> OffersForContracts { get; private set; }
    public IEnumerable<Review> ReviewsAsClient { get; private set; }
    public IEnumerable<Review> ReviewsAsProvider { get; private set; }

    public ViewUserModel(ILogger<ViewUserModel> logger,
        IJobService jobService,
        IUserService userService,
        IOfferService offerService,
        IContractService contractService,
        IReviewService reviewService)
    {
        _logger = logger;
        _jobService = jobService;
        _userService = userService;
        _offerService = offerService;
        _contractService = contractService;
        _reviewService = reviewService;
    }
    
    public IActionResult OnGet(Guid? clientId = null)
    {
        if (!SessionLoggedIn) return RedirectToPage("Login");

        if (clientId == null)
        {
            Client = _userService.GetUserById(SessionId);
        }
        else
        {
            Client = _userService.GetUserById(clientId.Value);
        }
        Jobs = _jobService.ListJobsByUser(Client.Id).Where(x => x.JobState == State.Open);
        
        var openContracts = new List<Contract>();
        var closedContracts = new List<Contract>();
        var jobIds = new List<Guid>();
        var offerIds = new List<Guid>();
        foreach (var contract in _contractService.ListContracts(Client.Id))
        {
            if(contract.ContractState == State.Open) openContracts.Add(contract);
            else closedContracts.Add(contract);
            jobIds.Add(contract.JobId);
            offerIds.Add(contract.OfferId);
        }
        OpenContracts = openContracts;
        ClosedContracts = closedContracts;
        JobsForContracts = _jobService.ListJobsByIDs(jobIds);
        OffersForContracts = _offerService.ListOffersByIDs(offerIds);
        ReviewsAsClient = _reviewService.ListReviews(Client.Id, ReviewType.Client);
        ReviewsAsProvider = _reviewService.ListReviews(Client.Id, ReviewType.Provider);

        return Page();
    }
}