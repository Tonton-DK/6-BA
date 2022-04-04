using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IReviewService : IBaseService
{
    public Review? CreateReview(Review review);
    public Review? GetReviewById(Guid id);
    public IEnumerable<Review> ListReviews(Guid userId, ReviewType type);
    public double GetRating(Guid userId, ReviewType type);
    public Review? UpdateReview(Review review);
    public bool DeleteReview(Guid id);
}