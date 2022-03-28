using ClassLibrary;

namespace OfferService.Interfaces;

public interface IDataProvider
{
    public List<Offer> GetOffers();
    public Offer GetOffer(int OfferId);
}