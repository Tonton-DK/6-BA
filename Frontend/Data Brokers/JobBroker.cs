using ClassLibrary.Classes;
using ClassLibrary.Interfaces;

namespace Frontend.Data_Brokers;

public class JobBroker : IJobService
{
    private static readonly HttpClient Client = new HttpClient();
    
    public IEnumerable<Job> Get()
    {
        var uri = "http://job-service:80/JobService";
        var t = WebApiClient<Job[]>(uri, Client);
        if (t != null) return new List<Job>(t.Result);
        return null;
    }

    public IEnumerable<Category> ListCategories()
    {
        var uri = "http://job-service:80/JobService/ListCategories";
        var t = WebApiClient<Category[]>(uri, Client);
        if (t != null) return new List<Category>(t.Result);
        return null;
    }

    public Job? CreateJob(Job job)
    {
        return null;
    }

    public Job? GetJobById(Guid id)
    {
        return null;
    }

    public IEnumerable<Job> ListJobs(Filter filter)
    {
        return new List<Job>();
    }

    public Job? UpdateJob(Job job)
    {
        return null;
    }

    public bool DeleteJobById(Guid id)
    {
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
}