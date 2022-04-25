using System.Text;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace ClassLibrary.Data_Brokers;

public class OfferBroker : BaseBroker, IOfferService
{
    private const string baseUri = "http://offer-service:80/OfferService";
    
    public bool Get()
    {
        var t = Get<bool>(baseUri+"/Get");
        if (t != null) return t.Result;
        return false;
    }

    public Offer? CreateOffer(Offer offer)
    {
        var content = new StringContent(JsonConvert.SerializeObject(offer), Encoding.UTF8, "application/json");
        var t = Post<Offer>(baseUri+"/CreateOffer", content);
        if (t != null) return t.Result;
        return null;
    }

    public Offer? GetOfferById(Guid id)
    {
        var t = Get<Offer>(baseUri+"/GetOfferById/"+id);
        if (t != null) return t.Result;
        return null;
    }

    public IEnumerable<Offer> ListOffersForJob(Guid jobId)
    {
        var t = Get<Offer[]>(baseUri+"/ListOffersForJob/"+jobId);
        if (t != null) return new List<Offer>(t.Result);
        return null;
    }

    public IEnumerable<Offer> ListOffersForUser(Guid userId)
    {
        var t = Get<Offer[]>(baseUri+"/ListOffersForUser/"+userId);
        if (t != null) return new List<Offer>(t.Result);
        return null;
    }

    public IEnumerable<Offer> ListOffersByIDs(IEnumerable<Guid> offerIds)
    {
        var uri = baseUri + "/ListOffersByIDs";
        var content = new StringContent(JsonConvert.SerializeObject(offerIds), Encoding.UTF8, "application/json");
        var t = Post<Offer[]>(uri, content);
        if (t != null) return new List<Offer>(t.Result);
        return null;
    }

    public Offer? UpdateOffer(Offer offer)
    {
        var content = new StringContent(JsonConvert.SerializeObject(offer), Encoding.UTF8, "application/json");
        var t = Put<Offer>(baseUri+"/UpdateOffer", content);
        if (t != null) return t.Result;
        return null;
    }

    public bool DeleteOffer(Guid id)
    {
        var t = Delete<bool>(baseUri+"/DeleteOffer/"+id);
        if (t != null) return t.Result;
        return false;
    }

    public Contract? AcceptOffer(Guid id)
    {
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var t = Put<Contract?>(baseUri+"/AcceptOffer/"+id, content);
        if (t != null) return t.Result;
        return null;
    }

    public Offer? CreateCounterOffer(Guid id, Offer counterOffer)
    {
        var content = new StringContent(JsonConvert.SerializeObject(counterOffer), Encoding.UTF8, "application/json");
        var t = Put<Offer?>(baseUri+"/CreateCounterOffer/"+id, content);
        if (t != null) return t.Result;
        return null;
    }

    public bool DeclineOffer(Guid id)
    {
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var t = Put<bool>(baseUri+"/DeclineOffer/"+id, content);
        if (t != null) return t.Result;
        return false;
    }
}