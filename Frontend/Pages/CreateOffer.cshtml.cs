using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class CreateOfferModel : PageModel
{
    private readonly ILogger<CreateOfferModel> _logger;
    
    private readonly IUserService _userService;
    private readonly IJobService _jobService;
    private readonly IOfferService _offerService;
    
    public Dictionary<Type, bool> ServiceStatus { get; private set; }
    
    public User Client { get; private set; }
    public User Provider { get; private set; }
    public Job Job { get; private set; }
    public Offer? PreviousOffer { get; private set; }
    public Offer Offer { get; private set; }
    
    public CreateOfferModel(ILogger<CreateOfferModel> logger, 
        IUserService userService, 
        IJobService jobService, 
        IOfferService offerService)
    {
        _logger = logger;
        _userService = userService;
        _jobService = jobService;
        _offerService = offerService;
        ServiceStatus = new Dictionary<Type, bool>();
    }
    
    public void OnGet()
    {
        //TODO: Set Instance variables OnGet
    }

    [HttpPost]
    public void OnPost()
    {
        
    }
}