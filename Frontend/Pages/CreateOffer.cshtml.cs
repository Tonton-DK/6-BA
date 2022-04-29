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
    }
    
    public Guid JobId { get; set; }
    public Guid ProviderId { get; set;}
    
    [BindProperty]
    public Offer Offer { get; private set; }
    
    public Job Job { get; set;}
    
    public IActionResult OnGet(Guid jobId)
    {
        Instantiate();
        Job = _jobService.GetJobById(jobId);
        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        Instantiate();
        
        Offer.JobId = JobId;
        Offer.ProviderId = ProviderId;
        Offer.PreviousOfferId = null;

        _offerService.CreateOffer(Offer);
        return RedirectToPage("Index");
    }
}