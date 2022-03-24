using ClassLibrary;
using Frontend.Data_Providers;

namespace Frontend.Services;

public class UserService
{
    public IEnumerable<User>? GetUsers()
    {
        var uri = "http://user-service:80/UserService";
        return new UserBroker().GetUsers(uri);
    }
}