using ClassLibrary.Classes;
using ClassLibrary.Interfaces;

namespace Frontend.Data_Brokers;

public class UserBroker : BaseBroker, IUserService
{
    private static readonly HttpClient Client = new HttpClient();
    private const string Uri = "http://user-service:80/UserService";

    // In order to display the forecasts on our page, we need to get them from the API
    public IEnumerable<User> Get()
    {
        var uri = "http://user-service:80/UserService";
        var t = Get<User[]>(uri, Client);
        if (t != null) return new List<User>(t.Result);
        return null;
    }
}