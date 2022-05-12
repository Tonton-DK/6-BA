using System.Net;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

[IgnoreAntiforgeryToken]
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
    }

    [BindProperty]
    public Offer Offer { get; set; }
    
    public Job Job { get; set;}
    
    [BindProperty]
    public Guid JobId { get; set;}
    
    public IActionResult OnGet(Guid jobId)
    {
        if (!SessionLoggedIn) return RedirectToPage("Login");
        
        Job = _jobService.GetJobById(jobId);
        JobId = Job.Id;
        
        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        if (!SessionLoggedIn) return RedirectToPage("Login");
        
        Offer.JobId = JobId;
        
        //TODO: Set previous offer id
        Offer.PreviousOfferId = Guid.Empty;
        
        //TODO: If any previous offers set provider id to same as this.
        Offer.ProviderId = SessionId;
        
        _offerService.CreateOffer(Offer);
        return RedirectToPage("ViewTask", new {jobId = JobId});
    }
}