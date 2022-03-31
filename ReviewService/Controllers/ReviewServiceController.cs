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

    public Review? CreateReview(Review review)
    {
        throw new NotImplementedException();
    }

    public Review? GetReviewById(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Review> ListReviews(Guid userId, ReviewType type)
    {
        throw new NotImplementedException();
    }

    public double GetRating(Guid userId, ReviewType type)
    {
        throw new NotImplementedException();
    }

    public Review? UpdateReview(Review review)
    {
        throw new NotImplementedException();
    }

    public bool DeleteReview(Guid id)
    {
        throw new NotImplementedException();
    }
}
