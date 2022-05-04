using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

public class ViewJobModel : LayoutModel
{
    private readonly ILogger<ViewJobModel> _logger;
    private readonly IJobService _jobService;
    private readonly IUserService _userService;
    private readonly IOfferService _offerService;
    
    [BindProperty]
    public Job Job { get; private set; }
    
    [BindProperty]
    public User Client { get; private set; }
    
    public bool IsOwner { get; private set; }
    public IEnumerable<Offer> Offers { get; private set; }
    public IEnumerable<User> Clients { get; private set; }
    
    public ViewJobModel(ILogger<ViewJobModel> logger,
        IJobService jobService,
        IUserService userService,
        IOfferService offerService)
    {
        _logger = logger;
        _jobService = jobService;
        _userService = userService;
        _offerService = offerService;
    }

    public IActionResult OnGet(Guid jobId)
    {
        Job = _jobService.GetJobById(jobId);
        Client = _userService.GetUserById(Job.ClientId);

        if (SessionLoggedIn)
        {
            var userId = SessionId;
            if (Client.Id.Equals(userId))
            {
                IsOwner = true;
                Offers = _offerService.ListOffersForJob(jobId).Where(x => x.State == State.Open); 
                var clientIds = Offers.Select(job => job.ProviderId).ToList();
                Clients = _userService.ListUsersByIDs(clientIds);
            }
        }
        
        return Page();
    }
    
    public IActionResult OnPostAccept(Guid jobId, Guid offerId)
    {
        _logger.Log(LogLevel.Warning, "OnPostAccept");
        var contract = _offerService.AcceptOffer(offerId);
        if (contract != null)
        {
            return RedirectToPage("ViewContract", new {contractId = contract.Id});
        }
        return OnGet(jobId);
    }
    
    public IActionResult OnPostCounteroffer(Guid jobId, Guid offerId)
    {
        _logger.Log(LogLevel.Warning, "OnPostCounteroffer");
        //var contract = _offerService.CreateCounterOffer();
        return OnGet(jobId);
    }
    
    public IActionResult OnPostDecline(Guid jobId, Guid offerId)
    {
        _logger.Log(LogLevel.Warning, "OnPostAccept");
        var result = _offerService.DeclineOffer(offerId);
        return OnGet(jobId);
    }
}