namespace Frontend.Data_Brokers;

public class BaseBroker
{
    private static HttpClient GetClient()
    {
        HttpClientHandler clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        HttpClient client = new HttpClient(clientHandler);
        return client;
    }

    private static async Task<T>? GetResponse<T>(HttpResponseMessage httpResponse)
    {
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

    protected static async Task<T>? Get<T>(string uri, HttpClient httpClient)
    {
        var client = GetClient();
        using var httpResponse = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
        return await GetResponse<T>(httpResponse);
    }

    protected static async Task<T>? Post<T>(string uri, HttpClient httpClient, StringContent content)
    {
        var client = GetClient();
        using var httpResponse = await client.PostAsync(uri, content);
        return await GetResponse<T>(httpResponse);
    }
    
    protected static async Task<T>? Put<T>(string uri, HttpClient httpClient, StringContent content)
    {
        var client = GetClient();
        using var httpResponse = await client.PutAsync(uri, content);
        return await GetResponse<T>(httpResponse);
    }
    
    protected static async Task<T>? Delete<T>(string uri, HttpClient httpClient)
    {
        var client = GetClient();
        using var httpResponse = await client.DeleteAsync(uri);
        return await GetResponse<T>(httpResponse);
    }
}