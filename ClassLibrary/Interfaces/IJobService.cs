using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IJobService
{
    public IEnumerable<Category> ListCategories();
    public Job? CreateJob(Job job);
    public Job? GetJobById(Guid id);
    public IEnumerable<Job> ListJobs(Filter filter);
    public Job? UpdateJob(Job job);
    public bool DeleteJobById(Guid id);
}