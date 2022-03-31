using System.Text;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace Frontend.Data_Brokers;

public class OfferBroker : BaseBroker, IOfferService
{
    private static readonly HttpClient Client = new HttpClient();
    private const string Uri = "http://offer-service:80/OfferService";

    // In order to display the forecasts on our page, we need to get them from the API

    public Offer? CreateOffer(Offer offer)
    {
        var content = new StringContent(JsonConvert.SerializeObject(offer), Encoding.UTF8, "application/json");
        var t = Post<Offer>(Uri+"/CreateOffer", Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public Offer? GetOfferById(Guid id)
    {
        var t = Get<Offer>(Uri+"/GetOfferById/"+id, Client);
        if (t != null) return t.Result;
        return null;
    }

    public IEnumerable<Offer> ListOffersForJob(Guid jobId)
    {
        var t = Get<Offer[]>(Uri+"/ListOffersForJob/"+jobId, Client);
        if (t != null) return new List<Offer>(t.Result);
        return null;
    }

    public IEnumerable<Offer> ListOffersForUser(Guid userId)
    {
        var t = Get<Offer[]>(Uri+"/ListOffersForUser/"+userId, Client);
        if (t != null) return new List<Offer>(t.Result);
        return null;
    }

    public Offer? UpdateOffer(Offer offer)
    {
        var content = new StringContent(JsonConvert.SerializeObject(offer), Encoding.UTF8, "application/json");
        var t = Put<Offer>(Uri+"/UpdateOffer", Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public bool DeleteOffer(Guid id)
    {
        var t = Delete<bool>(Uri+"/DeleteOffer/"+id, Client);
        if (t != null) return t.Result;
        return false;
    }

    public bool AcceptOffer(Guid id)
    {
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var t = Put<bool>(Uri+"/AcceptOffer/"+id, Client, content);
        if (t != null) return t.Result;
        return false;
    }

    public Offer? CreateCounterOffer(Guid id, Offer counterOffer)
    {
        var content = new StringContent(JsonConvert.SerializeObject(counterOffer), Encoding.UTF8, "application/json");
        var t = Put<Offer?>(Uri+"/AcceptOffer/"+id, Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public bool DeclineOffer(Guid id)
    {
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var t = Put<bool>(Uri+"/DeclineOffer/"+id, Client, content);
        if (t != null) return t.Result;
        return false;
    }
}