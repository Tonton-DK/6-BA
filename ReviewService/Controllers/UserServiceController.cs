using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ReviewService.Interfaces;

namespace ReviewService.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewServiceController : ControllerBase, IReviewService
{
    private readonly ILogger<ReviewServiceController> _logger;
    private readonly IDataProvider _dataProvider;

    public ReviewServiceController(ILogger<ReviewServiceController> logger, IDataProvider dataProvider)
    {
        _logger = logger;
        _dataProvider = dataProvider;
    }

    [HttpGet]
    public IEnumerable<Review> Get()
    {
        var reviews = _dataProvider.GetReviews();
        return reviews.ToArray();
    }

    [HttpGet("GetByName/{name}")]
    public IActionResult GetByName(string name)
    {
        return Ok("Name: " + name);
    }

    [HttpGet("GetById/{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok("Id: " + id);
    }

    public Review? CreateProfile(Review profile)
    {
        throw new NotImplementedException();
    }

    public Review? GetProfileById(Guid id, bool withCV)
    {
        throw new NotImplementedException();
    }

    public Review? UpdateProfile(Review profile)
    {
        throw new NotImplementedException();
    }

    public bool DeleteProfileById(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool ValidateProfile(string email, string password)
    {
        throw new NotImplementedException();
    }

    public bool ChangePassword(Guid id, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }
}
