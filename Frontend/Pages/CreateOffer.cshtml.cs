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
    
    public Offer Offer { get; private set; }
    
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
    public int Price { get; set; }
    [BindProperty]
    public string Duration { get; set;}
    [BindProperty]
    public DateTime Date { get; set;}
    [BindProperty]
    public string Comment { get; set;}

    [BindProperty]
    public Job Job { get; set;}
    
    public IActionResult OnGet(Guid jobId)
    {
        Instantiate();
        Job = _jobService.GetJobById(jobId);
        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        Offer = new Offer(Guid.Empty, JobId, ProviderId, Price, Duration, Date, State.Open, Comment);
        _offerService.CreateOffer(Offer);
        return RedirectToPage("Index");
    }
}