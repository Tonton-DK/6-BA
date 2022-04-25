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
        Instantiate();
        Job = _jobService.GetJobById(jobId);
        Client = _userService.GetUserById(Job.ClientId);

        if (HttpContext.Session.GetInt32(SessionLoggedInKey) == 1)
        {
            var userId = new Guid(HttpContext.Session.GetString(SessionIdKey));
            if (Client.Id.Equals(userId))
            {
                IsOwner = true;
                Offers = _offerService.ListOffersForJob(jobId); 
            }
        }
        
        return Page();
    }
}