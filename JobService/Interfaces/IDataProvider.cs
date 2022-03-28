using ClassLibrary.Classes;

namespace JobService.Interfaces;

public interface IDataProvider
{
    public List<Job> GetJobs();
}