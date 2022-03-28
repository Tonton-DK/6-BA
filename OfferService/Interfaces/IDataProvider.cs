using ClassLibrary.Classes;

namespace OfferService.Interfaces;

public interface IDataProvider
{
    public List<Offer> GetOffers(Guid JobId);
    public Offer GetOffer(int OfferId);
}