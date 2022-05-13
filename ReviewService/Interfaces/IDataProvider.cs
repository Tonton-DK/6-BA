using ClassLibrary.Classes;

namespace ReviewService.Interfaces;

public interface IDataProvider
{
    public Review? Create(Review review);
    public Review? Get(Guid id);
    public Review? Get(Guid contractId, Guid creatorId);
    public IEnumerable<Review> List(Guid userId);
    public Review? Update(Review review);
    public bool Delete(Guid id);
}