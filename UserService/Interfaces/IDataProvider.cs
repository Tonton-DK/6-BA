using ClassLibrary;

namespace UserService.Interfaces;

public interface IDataProvider
{
    public List<User> GetUsers();
}