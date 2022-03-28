using System.Text;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace Frontend.Data_Brokers;

public class JobBroker : BaseBroker, IJobService
{
    private static readonly string baseUri = "http://job-service:80/JobService";
    private static readonly HttpClient Client = new HttpClient();
    
    public IEnumerable<Job> Get()
    {
        var uri = baseUri;
        var t = WebApiClient<Job[]>(uri, Client);
        if (t != null) return new List<Job>(t.Result);
        return null;
    }

    public IEnumerable<Category> ListCategories()
    {
        var uri = baseUri + "/ListCategories";
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
        var uri = baseUri + "/ListJobs";
        var content = new StringContent(JsonConvert.SerializeObject(filter), Encoding.UTF8, "application/json");
        var t = WebApiClient<Job[]>(uri, Client, content);
        if (t != null) return new List<Job>(t.Result);
        return null;
    }

    public Job? UpdateJob(Job job)
    {
        return null;
    }

    public bool DeleteJobById(Guid id)
    {
        return false;
    }
}