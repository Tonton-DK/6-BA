using ClassLibrary;

namespace Backend.Interfaces;

public interface IDataProvider
{
    public List<User> GetUsers();
}