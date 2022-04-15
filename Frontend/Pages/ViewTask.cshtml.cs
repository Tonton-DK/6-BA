using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class ViewJobModel : PageModel
{
    private readonly ILogger<ViewJobModel> _logger;
    
    private readonly IJobService _jobService;
    private readonly IUserService _userService;
    private readonly IOfferService _offerService;
    
    public Dictionary<Type, bool> ServiceStatus { get; private set; }
    
    public Job Job { get; private set; }
    public User Client { get; private set; }
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
        ServiceStatus = new Dictionary<Type, bool>();
    }
    public void OnGet()
    {
        
    }
}