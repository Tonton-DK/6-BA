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

    public OfferServiceController(ILogger<OfferServiceController> logger, IDataProvider dataProvider)
    {
        _logger = logger;
        _dataProvider = dataProvider;
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
        var offers = _dataProvider.List(jobId);
        return offers.ToArray();
    }

    [HttpGet("ListOffersForUser/{userId}")]
    public IEnumerable<Offer> ListOffersForUser(Guid userId)
    {
        throw new NotImplementedException();
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
    public bool AcceptOffer(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("CreateCounterOffer/{id}")]
    public Offer? CreateCounterOffer(Guid id, Offer counterOffer)
    {
        throw new NotImplementedException();
    }
    
    [HttpDelete("DeclineOffer/{id}")]
    public bool DeclineOffer(Guid id)
    {
        throw new NotImplementedException();
    }
}
