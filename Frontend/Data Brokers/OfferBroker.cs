using ClassLibrary;
using ClassLibrary.Interfaces;

namespace Frontend.Data_Brokers;

public class OfferBroker : IOfferService
{
    private static readonly HttpClient Client = new HttpClient();
    
    // In order to display the forecasts on our page, we need to get them from the API
    public IEnumerable<Offer> Get()
    {
        var uri = "http://offer-service:80/OfferService";
        var t = WebApiClient(uri, Client);
        if (t != null) return new List<Offer>(t.Result);
        return null;
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