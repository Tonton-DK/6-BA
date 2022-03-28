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

    [HttpGet]
    public IEnumerable<Offer> Get(Guid jobId)
    {
        var offers = _dataProvider.List(jobId);
        return offers.ToArray();
    }

    [HttpGet("GetById/{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok("Id: " + id);
    }

    public IEnumerable<Offer> Get()
    {
        throw new NotImplementedException();
    }
}
