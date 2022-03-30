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

    public Offer? Create(Offer offer)
    {
        var content = new StringContent(JsonConvert.SerializeObject(offer), Encoding.UTF8, "application/json");
        var t = Post<Offer>(Uri+"/Create", Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public Offer? Get(Guid id)
    {
        var t = Get<Offer>(Uri+"/Get/"+id, Client);
        if (t != null) return t.Result;
        return null;
    }

    public IEnumerable<Offer> List(Guid jobId)
    {
        var t = Get<Offer[]>(Uri+"/List/"+jobId, Client);
        if (t != null) return new List<Offer>(t.Result);
        return null;
    }

    public Offer? Update(Offer offer)
    {
        var content = new StringContent(JsonConvert.SerializeObject(offer), Encoding.UTF8, "application/json");
        var t = Put<Offer>(Uri+"/Update", Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public bool Delete(Guid id)
    {
        var t = Delete<bool>(Uri+"/Delete/"+id, Client);
        if (t != null) return t.Result;
        return false;
    }
}