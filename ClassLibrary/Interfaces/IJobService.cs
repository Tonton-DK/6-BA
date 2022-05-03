using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IJobService : IBaseService
{
    public IEnumerable<Category> ListCategories();
    public Job? CreateJob(Job job);
    public Job? GetJobById(Guid id);
    public IEnumerable<Job> ListJobs(Filter filter);
    public IEnumerable<Job> ListJobsByUser(Guid userId);
    public IEnumerable<Job> ListJobsByIDs(IEnumerable<Guid> jobIds);
    public Job? UpdateJob(Job job);
    public bool DeleteJobById(Guid id);
    public Job? CloseJobById(Guid id);
}