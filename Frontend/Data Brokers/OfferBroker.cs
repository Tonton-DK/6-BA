using ClassLibrary.Classes;
using ClassLibrary.Interfaces;

namespace Frontend.Data_Brokers;

public class OfferBroker : IOfferService
{
    private static readonly HttpClient Client = new HttpClient();
    private const string Uri = "http://offer-service:80/OfferService";

    // In order to display the forecasts on our page, we need to get them from the API
    public Offer? Create(Offer offer)
    {
        throw new NotImplementedException();
    }

    public Offer? Get(Guid id)
    {
        var t = WebApiClient(Uri+id, Client);
        if (t != null) return t.Result;
        return null;
    }

    public IEnumerable<Offer> List(Guid jobId)
    {
        var t = WebApiClient(Uri+jobId, Client);
        if (t != null) return new List<Offer>(t.Result);
        return null;
    }

    public Offer? Update(Offer offer)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    private static async Task<Offer[]>? WebApiClient(string uri, HttpClient httpClient)
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        HttpClient client = new HttpClient(clientHandler);
            
        using var httpResponse = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
        httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299

        try
        {
            return await httpResponse.Content.ReadAsAsync<Offer[]>();
        }
        catch // Could be ArgumentNullException or UnsupportedMediaTypeException
        {
            Console.WriteLine("HTTP Response was invalid or could not be deserialized.");
        }

        return null;
    }
}