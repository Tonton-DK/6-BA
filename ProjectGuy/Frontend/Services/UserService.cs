using ClassLibrary;
using Frontend.Data_Providers;

namespace Frontend.Services;

public class UserService
{
    public IEnumerable<User>? GetUsers()
    {
        var uri = "https://localhost:7255/UserService";
        return new UserBroker().GetUsers(uri);
    }
}