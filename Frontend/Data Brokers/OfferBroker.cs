using System.Text;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace Frontend.Data_Brokers;

public class OfferBroker : IOfferService
{
    private static readonly HttpClient Client = new HttpClient();
    private const string Uri = "http://offer-service:80/OfferService";

    // In order to display the forecasts on our page, we need to get them from the API

    public Offer? Create(Offer offer)
    {
        var content = new StringContent(JsonConvert.SerializeObject(offer), Encoding.UTF8, "application/json");
        
        var t = WebApiClient<Offer>(Uri+"/Create", Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public Offer? Get(Guid id)
    {
        var t = WebApiClient<Offer>(Uri+"/Get/"+id, Client);
        if (t != null) return t.Result;
        return null;
    }

    public IEnumerable<Offer> List(Guid jobId)
    {
        var t = WebApiClient<Offer[]>(Uri+"/List/"+jobId, Client);
        if (t != null) return new List<Offer>(t.Result);
        return null;
    }

    public Offer? Update(Offer offer)
    {
        var content = new StringContent(JsonConvert.SerializeObject(offer), Encoding.UTF8, "application/json");
        
        var t = WebApiClient<Offer>(Uri+"/Update", Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public bool Delete(Guid id)
    {
        var t = WebApiClient<bool>(Uri+"/Delete/"+id, Client);
        if (t != null) return t.Result;
        return false;
    }

    private static async Task<T>? WebApiClient<T>(string uri, HttpClient httpClient)
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        HttpClient client = new HttpClient(clientHandler);
            
        using var httpResponse = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
        httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299

        try
        {
            return await httpResponse.Content.ReadAsAsync<T>();
        }
        catch // Could be ArgumentNullException or UnsupportedMediaTypeException
        {
            Console.WriteLine("HTTP Response was invalid or could not be deserialized.");
        }

        return default(T);
    }
    
    private static async Task<T>? WebApiClient<T>(string uri, HttpClient httpClient, StringContent content)
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        HttpClient client = new HttpClient(clientHandler);

        using var httpResponse = await client.PostAsync(uri, content);
        httpResponse.EnsureSuccessStatusCode(); // throws if not 200-299

        try
        {
            return await httpResponse.Content.ReadAsAsync<T>();
        }
        catch // Could be ArgumentNullException or UnsupportedMediaTypeException
        {
            Console.WriteLine("HTTP Response was invalid or could not be deserialized.");
        }

        return default(T);
    }
}