using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthCheckController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IJobService _jobService;
    private readonly IOfferService _offerService;
    private readonly IContractService _contractService;
    private readonly IReviewService _reviewService;
    
    private User Client { get; set; }
    private User Provider { get; set; }
    private IEnumerable<Category> Categories { get; set; }
    private Job Job { get; set; }
    private IEnumerable<Job> Jobs { get; set; }
    private IEnumerable<Job> ClientJobs { get; set; }
    private Offer Offer { get; set; }
    private Offer CounterOffer { get; set; }
    private IEnumerable<Offer> Offers { get; set; }
    private Contract Contract { get; set; }
    private IEnumerable<Contract> Contracts { get; set; }
    private Review Review { get; set; }
    private IEnumerable<Review> Reviews { get; set; }
    private decimal ClientRating { get; set; }
    private decimal ProviderRating { get; set; }
    
    public HealthCheckController( 
        IUserService userService, 
        IJobService jobService, 
        IOfferService offerService, 
        IContractService contractService, 
        IReviewService reviewService)
    {
        _userService = userService;
        _jobService = jobService;
        _offerService = offerService;
        _contractService = contractService;
        _reviewService = reviewService;
    }

    [HttpGet("Services")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Services()
    {
        _userService.Get();
        _jobService.Get();
        _offerService.Get();
        _contractService.Get();
        _reviewService.Get();
        return Ok();
    }

    [HttpGet("TestClients")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult TestClients()
    {
        var client1 = new UserCreator(Guid.Empty, "1", "Client 1", "", "", "https://upload.wikimedia.org/wikipedia/commons/0/08/Un1.svg",true, "1");
        _userService.CreateUser(client1);
        var client2 = new UserCreator(Guid.Empty, "2", "Client 2", "", "", "https://upload.wikimedia.org/wikipedia/commons/3/3b/Deux.svg",true, "2");
        _userService.CreateUser(client2);
        return Ok();
    }
    
    [HttpGet("ServiceFlow")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult ServiceFlow()
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
        
        return Ok();
    }
}