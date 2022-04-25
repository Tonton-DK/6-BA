using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Frontend.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Pages;

public class IndexModel : LayoutModel
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
    public IEnumerable<Job> ClientJobs { get; private set; }
    public Offer Offer { get; private set; }
    public Offer CounterOffer { get; private set; }
    public IEnumerable<Offer> Offers { get; private set; }
    public Contract Contract { get; private set; }
    public IEnumerable<Contract> Contracts { get; private set; }
    public Review Review { get; private set; }
    public IEnumerable<Review> Reviews { get; private set; }
    public decimal ClientRating { get; private set; }
    public decimal ProviderRating { get; private set; }

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
    
    public IActionResult OnGet()
    {
        Instantiate();
        GetServiceStatus();
        TestFullServiceFlow();
        return Page();
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
        var client = new UserCreator(Guid.Empty, "client@mail.dk", "Client", "Dude", "12345678", "/images/person.jpg",false, "secret");
        Client = _userService.CreateUser(client);
        Client = _userService.GetUserById(Client.Id);
        Client = _userService.ValidateUser(new LoginRequest(client.Email, client.Password));
        var provider = new UserCreator(Guid.Empty, "provider@mail.dk", "Provider", "Dude", "12345678", "/images/person.jpg",true, "secret");
        Provider = _userService.CreateUser(provider);
        Provider = _userService.GetUserById(Provider.Id);
        Provider = _userService.ValidateUser(new LoginRequest(provider.Email, provider.Password));

        Categories = _jobService.ListCategories();
        var address = new Address("Campusvej", "55", "5230");
        Job = new Job(Guid.Empty, "Dog walker", "Walk my dog, please. He is a good boy.", DateTime.Now, Categories.First(), address, Client.Id);
        Job = _jobService.CreateJob(Job);
        Job = _jobService.GetJobById(Job.Id);
        var filter = new Filter(null, null, null, "", "");
        Jobs = _jobService.ListJobs(filter);
        ClientJobs = _jobService.ListJobsByUser(Client.Id);

        Offer = new Offer(Guid.Empty, Job.Id, Provider.Id, 500, "2 Hours", DateTime.Now, State.Open, "Comment");
        Offer = _offerService.CreateOffer(Offer);
        Offer = _offerService.GetOfferById(Offer.Id);
        CounterOffer = new Offer(Guid.Empty, Job.Id, Provider.Id, 400, "1 Hours", DateTime.Now, State.Open, "Comment");
        CounterOffer = _offerService.CreateCounterOffer(Offer.Id, CounterOffer);
        Offers = _offerService.ListOffersForJob(Job.Id);
        Offers = _offerService.ListOffersForUser(Provider.Id);
        Contract = _offerService.AcceptOffer(CounterOffer.Id);
        
        Contract = _contractService.GetContractById(Contract.Id);
        Contracts = _contractService.ListContracts(Client.Id);
        _contractService.ConcludeContract(Contract.Id);
        
        Review = new Review(Guid.Empty, Contract.Id, Client.Id, Provider.Id, "Really good job.", 5, ReviewType.Provider);
        Review = _reviewService.CreateReview(Review);
        Review = new Review(Guid.Empty, Contract.Id, Client.Id, Provider.Id, "Okay job.", 3, ReviewType.Provider);
        Review = _reviewService.CreateReview(Review);
        Review = _reviewService.GetReviewById(Review.Id);
        Reviews = _reviewService.ListReviews(Provider.Id, ReviewType.All);
        ClientRating = _reviewService.GetRating(Client.Id, ReviewType.All);
        ProviderRating = _reviewService.GetRating(Provider.Id, ReviewType.All);
    }
}
