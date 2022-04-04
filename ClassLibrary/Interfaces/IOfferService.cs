using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IOfferService : IBaseService
{
    public Offer? CreateOffer(Offer offer);
    public Offer? GetOfferById(Guid id);
    public IEnumerable<Offer> ListOffersForJob(Guid jobId);
    public IEnumerable<Offer> ListOffersForUser(Guid userId);
    public Offer? UpdateOffer(Offer offer);
    public bool DeleteOffer(Guid id);
    public bool AcceptOffer(Guid id);
    public Offer? CreateCounterOffer(Guid id, Offer counterOffer);
    public bool DeclineOffer(Guid id);
}