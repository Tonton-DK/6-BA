using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

public class ViewContractModel : LayoutModel
{
    private readonly ILogger<ViewContractModel> _logger;
    
    private readonly IContractService _contractService;
    private readonly IUserService _userService;
    private readonly IJobService _jobService;
    private readonly IOfferService _offerService;
    private readonly IReviewService _reviewService;

    public bool ValidContract { get; set; }
    public Contract Contract { get; set; }
    public User Client { get; set; }
    public decimal ClientRating { get; set; }
    public User Provider { get; set; }
    public decimal ProviderRating { get; set; }
    public Job Job { get; set; }
    public Offer Offer { get; set; }

    [BindProperty] 
    public State State { get; set; }
    
    public ViewContractModel(ILogger<ViewContractModel> logger,
        IJobService jobService,
        IUserService userService,
        IContractService contractService,
        IReviewService reviewService,
        IOfferService offerService)
    {
        _logger = logger;
        _jobService = jobService;
        _userService = userService;
        _contractService = contractService;
        _reviewService = reviewService;
        _offerService = offerService;
    }

    public void TestContract()
    {
        var client = new UserCreator(Guid.Empty, "client@mail.dk", "Client", "Dude", "12345678", "/images/person.jpg",false, "secret");
        Client = _userService.CreateUser(client);
        var provider = new UserCreator(Guid.Empty, "provider@mail.dk", "Provider", "Dude", "12345678", "/images/person.jpg",true, "secret");
        Provider = _userService.CreateUser(provider);

        var categories = _jobService.ListCategories();
        var address = new Address("Campusvej", "55", "5230");
        Job = new Job(Guid.Empty, "Dog walker", "Walk my dog, please. He is a good boy.", DateTime.Now, categories.First(), address, Client.Id);
        Job = _jobService.CreateJob(Job);

        Offer = new Offer(Guid.Empty, Job.Id, Provider.Id, 500, "2 Hours", DateTime.Now, State.Open, "Comment");
        Offer = _offerService.CreateOffer(Offer);
        Contract = _offerService.AcceptOffer(Offer.Id);
    }
    
    public IActionResult OnGet(Guid contractId)
    {
        Instantiate();

        if (_contractService.GetContractById(contractId) == null)
        {
            TestContract();
            return Page();
        }
        Contract = _contractService.GetContractById(contractId);
        
        /*
        if (!SessionLoggedIn || !Contract.ClientId.Equals(new Guid(HttpContext.Session.GetString(SessionIdKey))) || !Contract.ProviderId.Equals(new Guid(HttpContext.Session.GetString(SessionIdKey))))
        {
            ViewData["Title"] = "Not allowed";
            ViewData["Message"] = "You are not allowed to view this contract.";
            ValidContract = false;
            return Page();
        }
        */
        
        ValidContract = true; 
        
        Client = _userService.GetUserById(Contract.ClientId);
        ClientRating = _reviewService.GetRating(Client.Id, ReviewType.Client);
        Provider = _userService.GetUserById(Contract.ProviderId);
        ProviderRating = _reviewService.GetRating(Provider.Id, ReviewType.Provider);
        Job = _jobService.GetJobById(Contract.JobId);
        Offer = _offerService.GetOfferById(Contract.OfferId);

        return Page();
    }
    
    public IActionResult OnPostCancel(Guid contractId)
    {
        Contract = _contractService.CancelContract(contractId);
        return OnGet(Contract.Id);
    }

    public IActionResult OnPostConclude(Guid contractId)
    {
        Contract = _contractService.ConcludeContract(contractId);
        return OnGet(Contract.Id);
    }
}