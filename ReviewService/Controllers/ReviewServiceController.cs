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

    [HttpPost("CreateReview")]
    public Review? CreateReview([FromBody]Review review)
    {
        return _dataProvider.Create(review);
    }

    [HttpGet("GetReviewById/{id}")]
    public Review? GetReviewById(Guid id)
    {
        return _dataProvider.Get(id);
    }

    [HttpGet("ListReviews/{id}")]
    public IEnumerable<Review> ListReviews(Guid userId, [FromBody]ReviewType type)
    {
        return _dataProvider.List(userId).Where(review => review.Type == type).ToList();
    }

    [HttpGet("GetRating/{id}")]
    public double GetRating(Guid userId, [FromBody]ReviewType type)
    {
        var reviews = _dataProvider.List(userId).Where(review => review.Type == type).ToList();
        return reviews.Aggregate<Review, double>(0, (current, review) => current + review.Rating) /reviews.Count;
    }

    [HttpPut("UpdateReview/{id}")]
    public Review? UpdateReview(Review review)
    {
        return _dataProvider.Update(review);
    }

    [HttpDelete("DeleteReview/{id}")]
    public bool DeleteReview(Guid id)
    {
        return _dataProvider.Delete(id);
        
    }
}
