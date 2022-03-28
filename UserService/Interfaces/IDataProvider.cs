using ClassLibrary.Classes;

namespace UserService.Interfaces;

public interface IDataProvider
{
    public List<User> GetUsers();
}