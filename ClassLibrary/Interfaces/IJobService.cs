using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IJobService
{
    public IEnumerable<Job> Get();
}