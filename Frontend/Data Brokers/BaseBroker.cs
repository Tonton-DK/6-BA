namespace Frontend.Data_Brokers;

public class BaseBroker
{
    protected static async Task<T>? WebApiClient<T>(string uri, HttpClient httpClient)
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

    protected static async Task<T>? WebApiClient<T>(string uri, HttpClient httpClient, StringContent content)
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