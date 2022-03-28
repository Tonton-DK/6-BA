using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IOfferService
{
    public Offer? Create(Offer offer);
    public Offer? Get(Guid id);
    public IEnumerable<Offer> List(Guid jobId);
    public Offer? Update(Offer offer);
    public bool Delete(Guid id);
}