using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using OfferService.Interfaces;

namespace OfferService.Controllers;

[ApiController]
[Route("[controller]")]
public class OfferServiceController : ControllerBase, IOfferService
{
    private readonly ILogger<OfferServiceController> _logger;
    private readonly IDataProvider _dataProvider;
    private readonly IJobService _jobService;
    private readonly IContractService _contractService;

    public OfferServiceController(ILogger<OfferServiceController> logger, IDataProvider dataProvider, 
        IJobService jobService, IContractService contractService)
    {
        _logger = logger;
        _dataProvider = dataProvider;
        _jobService = jobService;
        _contractService = contractService;
    }

    [HttpGet("Get")]
    public bool Get()
    {
        return true;
    }

    [HttpPost("CreateOffer")]
    public Offer? CreateOffer([FromBody]Offer offer)
    {
        return _dataProvider.Create(offer);
    }
    
    [HttpGet("GetOfferById/{id}")]
    public Offer? GetOfferById(Guid id)
    {
        return _dataProvider.Get(id);
    }
    
    [HttpGet("ListOffersForJob/{jobId}")]
    public IEnumerable<Offer> ListOffersForJob(Guid jobId)
    {
        var offers = _dataProvider.ListForJob(jobId);
        return offers.ToArray();
    }

    [HttpGet("ListOffersForUser/{userId}")]
    public IEnumerable<Offer> ListOffersForUser(Guid userId)
    {
        var offers = _dataProvider.ListForUser(userId);
        return offers.ToArray();
    }

    [HttpPost("ListOffersByIDs")]
    public IEnumerable<Offer> ListOffersByIDs([FromBody] IEnumerable<Guid> offerIds)
    {
        return _dataProvider.ListForIDs(offerIds);
    }

    [HttpPut("UpdateOffer")]
    public Offer? UpdateOffer([FromBody]Offer offer)
    {
        return _dataProvider.Update(offer);
    }

    [HttpDelete("DeleteOffer/{id}")]
    public bool DeleteOffer(Guid id)
    {
        return _dataProvider.Delete(id);
    }

    [HttpPut("AcceptOffer/{id}")]
    public Contract? AcceptOffer(Guid id)
    {
        // Accept offer
        var offer = _dataProvider.AcceptOffer(id);
        var job = _jobService.CloseJobById(offer.JobId);
        
        // Create contract
        var contractId = Guid.NewGuid(); 
        var jobId = offer.JobId; 
        var offerId = offer.Id;
        var clientId = job.ClientId;
        var providerId = offer.ProviderId;
        var creationDate = DateTime.Now;
        var contractState = State.Open;
        
        var contract = new Contract(contractId, jobId, offerId, clientId, providerId, creationDate, contractState);
        return _contractService.CreateContract(contract);
    }

    [HttpPut("CreateCounterOffer/{id}")]
    public Offer? CreateCounterOffer(Guid id, [FromBody]Offer counterOffer)
    {
        counterOffer.PreviousOfferId = id;
        return _dataProvider.Create(counterOffer);
    }
    
    [HttpPut("DeclineOffer/{id}")]
    public bool DeclineOffer(Guid id)
    {
        return _dataProvider.DeclineOffer(id);
    }
}
