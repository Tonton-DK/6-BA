using ClassLibrary.Classes;

namespace JobService.Interfaces;

public interface IDataProvider
{
    public Job? CreateJob(Job job);
    public Job? GetJob(Guid id);
    public List<Job> ListJobs(Filter filter);
    IEnumerable<Job> ListJobsByUser(Guid userId);
    IEnumerable<Job> ListJobsByIDs(IEnumerable<Guid> jobIds);
    public Job? UpdateJob(Job job);
    public bool DeleteJob(Guid id);
    public Category? CreateCategory(Category category);
    public Category GetCategory(Guid id);
    public List<Category> ListCategories();
    public Category UpdateCategory(Category category);
    public bool DeleteCategory(Guid id);
}