using ClassLibrary;
using ClassLibrary.Interfaces;

namespace Frontend.Data_Brokers;

public class UserBroker : IUserService
{
    private static readonly HttpClient Client = new HttpClient();
    
    // In order to display the forecasts on our page, we need to get them from the API
    public IEnumerable<User> Get()
    {
        var uri = "http://user-service:80/OfferOfferService";
        var t = WebApiClient(uri, Client);
        if (t != null) return new List<User>(t.Result);
        return null;
    }
    
    private static async Task<User[]>? WebApiClient(string uri, HttpClient httpClient)
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        HttpClient client = new HttpClient(clientHandler);
            
        using var httpResponse = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
        httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299

        try
        {
            return await httpResponse.Content.ReadAsAsync<User[]>();
        }
        catch // Could be ArgumentNullException or UnsupportedMediaTypeException
        {
            Console.WriteLine("HTTP Response was invalid or could not be deserialized.");
        }

        return null;
    }
}