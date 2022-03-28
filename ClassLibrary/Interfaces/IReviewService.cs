using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IReviewService
{
    public IEnumerable<Review> Get();
}