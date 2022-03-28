using ClassLibrary;

namespace JobService.Interfaces;

public interface IDataProvider
{
    public List<Job> GetJobs();
}