using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IOfferService
{
    public IEnumerable<Offer> Get();
}