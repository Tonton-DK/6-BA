using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Frontend.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    
    private readonly IUserService _userService;
    private readonly IJobService _jobService;
    private readonly IOfferService _offerService;
    private readonly IContractService _contractService;
    private readonly IReviewService _reviewService;

    public Dictionary<Type, bool> ServiceStatus { get; private set; }
    
    public User Client { get; private set; }
    public User Provider { get; private set; }
    public IEnumerable<Category> Categories { get; private set; }
    public Job Job { get; private set; }
    public IEnumerable<Job> Jobs { get; private set; }
    public Offer Offer { get; private set; }
    public IEnumerable<Offer> Offers { get; private set; }
    public Contract Contract { get; private set; }
    public IEnumerable<Contract> Contracts { get; private set; }
    public Review Review { get; private set; }
    public IEnumerable<Review> Reviews { get; private set; }
    public decimal Rating { get; private set; }

    public IndexModel(ILogger<IndexModel> logger, 
        IUserService userService, 
        IJobService jobService, 
        IOfferService offerService, 
        IContractService contractService, 
        IReviewService reviewService)
    {
        _logger = logger;
        _userService = userService;
        _jobService = jobService;
        _offerService = offerService;
        _contractService = contractService;
        _reviewService = reviewService;
        ServiceStatus = new Dictionary<Type, bool>();
    }

    public void OnGet()
    {
        GetServiceStatus();
        TestFullServiceFlow();
    }
    
    private void GetServiceStatus()
    {
        ServiceStatus.Add(_userService.GetType(), _userService.Get());
        ServiceStatus.Add(_jobService.GetType(), _jobService.Get());
        ServiceStatus.Add(_offerService.GetType(), _offerService.Get());
        ServiceStatus.Add(_contractService.GetType(), _contractService.Get());
        ServiceStatus.Add(_reviewService.GetType(), _reviewService.Get());
    }

    private void TestFullServiceFlow()
    {
        Client = new User(Guid.Empty, "client@mail.dk", "secret", "Client", "Dude", "12345678", false);
        Client = _userService.CreateUser(Client);
        Client = _userService.ValidateUser(new LoginData(Client.Email, Client.Password));
        Provider = new User(Guid.Empty, "provider@mail.dk", "secret", "Provider", "Dude", "12345678", true);
        Provider = _userService.CreateUser(Provider);
        Provider = _userService.ValidateUser(new LoginData(Provider.Email, Provider.Password));

        Categories = _jobService.ListCategories();
        var address = new Address("Campusvej", "55", "5230");
        Job = new Job(Guid.Empty, "Dog walker", "Walk my dog, please. He is a good boy.", DateTime.Now, Categories.First(), address, Client.Id);
        Job = _jobService.CreateJob(Job);
        var filter = new Filter(null, null, null, "", "");
        Jobs = _jobService.ListJobs(filter);
        Job = _jobService.GetJobById(Job.Id);
        
        Offer = new Offer(Guid.Empty, Job.Id, Provider.Id, 500, "2 Hours", DateTime.Now, State.Open);
        Offer = _offerService.CreateOffer(Offer);
        Offer = _offerService.GetOfferById(Offer.Id);
        Offers = _offerService.ListOffersForJob(Job.Id);
        Offers = _offerService.ListOffersForUser(Provider.Id);
        Contract = _offerService.AcceptOffer(Offer.Id);

        Contract = _contractService.GetContractById(Contract.Id);
        Contracts = _contractService.ListContracts(Client.Id);
        _contractService.ConcludeContract(Contract.Id);

        Review = new Review(Guid.Empty, Contract.Id, Client.Id, Provider.Id, "Really good job.", 5, ReviewType.Provider);
        Review = _reviewService.CreateReview(Review);
        Review = _reviewService.GetReviewById(Review.Id);
        Reviews = _reviewService.ListReviews(Provider.Id, ReviewType.All);
        Rating = _reviewService.GetRating(Provider.Id, ReviewType.All);
    }
}
