using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    public Review Review { get; set; }
    
    public bool Reviewed { get; set; }
    
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
        Review = new Review();
    }

    public IActionResult OnGet(Guid contractId)
    {
        Contract = _contractService.GetContractById(contractId);
        
        if (!SessionLoggedIn || 
            !Contract.ClientId.Equals(SessionId) && 
            !Contract.ProviderId.Equals(SessionId))
        {
            ViewData["Title"] = "Not allowed";
            ViewData["Message"] = "You are not allowed to view this contract.";
            ValidContract = false;
            return Page();
        }
        
        ValidContract = true; 
        
        Client = _userService.GetUserById(Contract.ClientId);
        ClientRating = _reviewService.GetRating(Client.Id, ReviewType.Client);
        Provider = _userService.GetUserById(Contract.ProviderId);
        ProviderRating = _reviewService.GetRating(Provider.Id, ReviewType.Provider);
        Job = _jobService.GetJobById(Contract.JobId);
        Offer = _offerService.GetOfferById(Contract.OfferId);

        Review.ContractId = contractId;
        Review.CreatorId = SessionId;
        Review.TargetId = SessionId == Client.Id ? Provider.Id : Client.Id;
        Review.Type = SessionId == Client.Id ? ReviewType.Provider : ReviewType.Client;

        Reviewed = _reviewService.GetReviewByCreatorId(contractId, SessionId) != null;
        
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

    public IActionResult OnPostReview()
    {
        _logger.Log(LogLevel.Warning, "Sending: " + JsonConvert.SerializeObject(Review));
        var review = _reviewService.CreateReview(Review);
        return OnGet(Review.ContractId);
    }
}