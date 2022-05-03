using System.Text;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace ClassLibrary.Data_Brokers;

public class JobBroker : BaseBroker, IJobService
{
    private static readonly string baseUri = "http://job-service:80/JobService";

    public bool Get()
    {
        var t = Get<bool>(baseUri+"/Get");
        if (t != null) return t.Result;
        return false;
    }
    
    public IEnumerable<Category> ListCategories()
    {
        var uri = baseUri + "/ListCategories";
        var t = Get<Category[]>(uri);
        if (t != null) return new List<Category>(t.Result);
        return null;
    }

    public Job? CreateJob(Job job)
    {
        var uri = baseUri + "/CreateJob";
        var content = new StringContent(JsonConvert.SerializeObject(job), Encoding.UTF8, "application/json");
        var t = Post<Job?>(uri, content);
        if (t != null) return t.Result;
        return null;
    }

    public Job? GetJobById(Guid id)
    {
        var uri = baseUri + "/GetJobById/" + id;
        var t = Get<Job?>(uri);
        if (t != null) return t.Result;
        return null;
    }

    public IEnumerable<Job> ListJobs(Filter filter)
    {
        var uri = baseUri + "/ListJobs";
        var content = new StringContent(JsonConvert.SerializeObject(filter), Encoding.UTF8, "application/json");
        var t = Post<Job[]>(uri, content);
        if (t != null) return new List<Job>(t.Result);
        return null;
    }

    public IEnumerable<Job> ListJobsByUser(Guid userId)
    {
        var uri = baseUri + "/ListJobsByUser/" + userId;
        var t = Get<Job[]>(uri);
        if (t != null) return new List<Job>(t.Result);
        return null;
    }

    public IEnumerable<Job> ListJobsByIDs(IEnumerable<Guid> jobIds)
    {
        var uri = baseUri + "/ListJobsByIDs";
        var content = new StringContent(JsonConvert.SerializeObject(jobIds), Encoding.UTF8, "application/json");
        var t = Post<Job[]>(uri, content);
        if (t != null) return new List<Job>(t.Result);
        return null;
    }

    public Job? UpdateJob(Job job)
    {
        var uri = baseUri + "/UpdateJob";
        var content = new StringContent(JsonConvert.SerializeObject(job), Encoding.UTF8, "application/json");
        var t = Put<Job?>(uri, content);
        if (t != null) return t.Result;
        return null;
    }

    public bool DeleteJobById(Guid id)
    {
        var uri = baseUri + "/DeleteJobById/" + id;
        var t = Delete<bool>(uri);
        if (t != null) return t.Result;
        return false;
    }

    public Job? CloseJobById(Guid id)
    {
        var uri = baseUri + "/CloseJobById/" + id;
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var t = Put<Job?>(uri, content);
        if (t != null) return t.Result;
        return null;
    }
}