using ClassLibrary.Classes;

namespace OfferService.Interfaces;

public interface IDataProvider
{
    public Offer Create(Offer offer);
    public Offer Get(Guid id);
    public List<Offer> List(Guid jobId);
    public Offer Update(Offer offer);
    public Boolean Delete(Guid id);
}