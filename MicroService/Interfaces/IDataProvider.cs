using ClassLibrary;

namespace MicroService.Interfaces;

public interface IDataProvider
{
    public List<User> GetUsers();
}