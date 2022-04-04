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
        // TODO: Add to IDataProvider as well?
        // We need to rethink this one. If i make an offer on your job, and then you make a counteroffer, i still need to get that counter offer.
        // Currently we can only collect offers for my job or offers i made.
        // We can't get counter offers to offers i made
        
        // New Strategy: Always save Provider as ProviderId on an offer
        // The Client ID is found via the Job itself
        // The first offer in a chain = providers offer
        // Every second offer in the chain = client offer
        var offers = _dataProvider.ListForUser(userId);
        return offers.ToArray();
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

    [HttpDelete("AcceptOffer/{id}")]
    public Contract? AcceptOffer(Guid id)
    {
        // TODO
        // Accept offer
        var offer = _dataProvider.AcceptOffer(id);
        var job = _jobService.GetJobById(offer.JobId);
        
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

    [HttpDelete("CreateCounterOffer/{id}")]
    public Offer? CreateCounterOffer(Guid id, [FromBody]Offer counterOffer)
    {
        // TODO: Is this needed? We could simply use CreateOffer and simply add "PreviousOfferId" before making the API call.
        // Good point
        counterOffer.PreviousOfferId = id;
        return _dataProvider.Create(counterOffer);
    }
    
    [HttpDelete("DeclineOffer/{id}")]
    public bool DeclineOffer(Guid id)
    {
        // TODO: Should we just delete declined offers?
        // We need to decline it, to show the offer maker that he was rejected
        return _dataProvider.DeclineOffer(id);
    }
}
