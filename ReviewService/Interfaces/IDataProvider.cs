using ClassLibrary.Classes;

namespace ReviewService.Interfaces;

public interface IDataProvider
{
    public Review? Create(Review review);
    public Review? Get(Guid id);
    public List<Review> List(Guid userId);
    public Review? Update(Review review);
    public bool Delete(Guid id);

}