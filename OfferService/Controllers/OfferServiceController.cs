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
    public IEnumerable<Offer> Get()
    {
        var offers = _dataProvider.GetOffers();
        return offers.ToArray();
    }

    [HttpGet("GetById/{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok("Id: " + id);
    } 
}
