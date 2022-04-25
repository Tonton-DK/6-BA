using ClassLibrary.Classes;

namespace OfferService.Interfaces;

public interface IDataProvider
{
    public Offer? Create(Offer offer);
    public Offer? Get(Guid id);
    public List<Offer> ListForJob(Guid jobId);
    public List<Offer> ListForUser(Guid userId);
    public List<Offer> ListForIDs(IEnumerable<Guid> offerIds);
    public Offer? Update(Offer offer);
    public bool Delete(Guid id);

    public Offer? AcceptOffer(Guid id);
    public bool DeclineOffer(Guid id);
}