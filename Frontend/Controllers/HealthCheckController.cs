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

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Get()
    {
        _userService.Get();
        _jobService.Get();
        _offerService.Get();
        _contractService.Get();
        _reviewService.Get();
        return Ok();
    }
}