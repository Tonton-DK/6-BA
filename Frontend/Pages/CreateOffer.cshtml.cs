using System.Net;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

public class CreateOfferModel : LayoutModel
{
    private readonly ILogger<CreateOfferModel> _logger;
    
    private readonly IJobService _jobService;
    private readonly IOfferService _offerService;

    public CreateOfferModel(ILogger<CreateOfferModel> logger,
        IJobService jobService,
        IOfferService offerService)
    {
        _logger = logger;
        _jobService = jobService;
        _offerService = offerService;
        Offer = new Offer();
        logger.Log(LogLevel.Warning,"Model instantiated");
    }

    [BindProperty]
    public Offer Offer { get; set; }
    
    public Job Job { get; set;}
    
    public IActionResult OnGet(Guid jobId)
    {
        Instantiate();
        _logger.Log(LogLevel.Warning, Offer.JobId.ToString());
        
        Job = _jobService.GetJobById(jobId);
        _logger.Log(LogLevel.Warning, Offer.JobId.ToString());
        
        Offer.JobId = Job.Id;
        _logger.Log(LogLevel.Warning, Offer.JobId.ToString());
        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        _logger.Log(LogLevel.Warning, Offer.JobId.ToString());
        
        Instantiate();
        _logger.Log(LogLevel.Warning, Offer.JobId.ToString());
        
        Offer.JobId = Job.Id;
        
        //TODO: If any previous offers set provider id to same as this.
        Offer.ProviderId = new Guid(HttpContext.Session.GetString(SessionIdKey));
        
        //TODO: Set previous offer id
        Offer.PreviousOfferId = Guid.Empty;

        _offerService.CreateOffer(Offer);
        return RedirectToPage("Index");
    }
}