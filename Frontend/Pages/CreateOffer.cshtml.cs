using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

public class CreateOfferModel : LayoutModel
{
    private readonly ILogger<CreateOfferModel> _logger;
    
    private readonly IOfferService _offerService;
    
    public Dictionary<Type, bool> ServiceStatus { get; private set; }
    
    public Offer Offer { get; private set; }
    
    public CreateOfferModel(ILogger<CreateOfferModel> logger,
        IOfferService offerService)
    {
        _logger = logger;
        _offerService = offerService;
        ServiceStatus = new Dictionary<Type, bool>();
    }
    
    public Guid JobId { get; set; }
    public Guid ProviderId { get; set;}
    [BindProperty]
    public int Price { get; set; }
    [BindProperty]
    public string Duration { get; set;}
    [BindProperty]
    public DateTime Date { get; set;}

    public IActionResult OnGet()
    {
        Instantiate();
        return Page();
    }
    
    public async Task<IActionResult> OnPost()
    {
        Offer = new Offer(Guid.Empty, JobId, ProviderId, Price, Duration, Date, State.Open);
        _offerService.CreateOffer(Offer);
        return RedirectToPage("Index");
    }
}