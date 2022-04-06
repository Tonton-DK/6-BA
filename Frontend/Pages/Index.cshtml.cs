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
    public IEnumerable<Job> Jobs { get; private set; }
    public IEnumerable<Offer> Offers { get; private set; }
    public IEnumerable<Contract> Contracts { get; private set; }

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

        /*
        User = _userService.ValidateUser(new LoginData("test@mail.dk", "secret"));
        Categories = _jobService.ListCategories();
        var data = new Filter(
            Guid.Parse("0ebbe367-300a-4c86-9070-d6e106d7e4b9"), 
            DateTime.Now.AddDays(-7), 
            DateTime.Now.AddDays(7), 
            "5000",
            "");
        Jobs = _jobService.ListJobs(data);
        Offers = TestOffers();
        Contracts = TestContracts();
        */
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
    }

    /*
    // Create test offer and load it afterwards
    IEnumerable<Offer>? TestOffers()
    {

        _offerService.CreateOffer(new Offer(
                Guid.Empty, 
                Jobs.ToArray()[0].Id, 
                User.Id, 
                400, 
                "2 Hours", 
                DateTime.Now));
        
        return _offerService.ListOffersForJob(Jobs.ToArray()[0].Id);
    }
    
    // Create test contracts and load it afterwards
    IEnumerable<Contract>? TestContracts()
    {

        _contractService.CreateContract(new Contract(
            Guid.Empty, 
            Jobs.ToArray()[0].Id, 
            Offers.ToArray()[0].Id, 
            User.Id, 
            User.Id, 
            DateTime.Now,
            State.Open));
        
        return _contractService.ListContracts(Jobs.ToArray()[0].Id);
    }
    */
}
