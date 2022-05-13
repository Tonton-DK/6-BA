using ClassLibrary.Classes;

namespace OfferService.Interfaces;

public interface IDataProvider
{
    public Offer? Create(Offer offer);
    public Offer? Get(Guid id);
    public IEnumerable<Offer> ListForJob(Guid jobId);
    public IEnumerable<Offer> ListForUser(Guid userId);
    public IEnumerable<Offer> ListForIDs(IEnumerable<Guid> offerIds);
    public Offer? Update(Offer offer);
    public bool Delete(Guid id);
    public Offer? AcceptOffer(Guid id);
    public bool DeclineOffer(Guid id);
}