using System.Text;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace Frontend.Data_Brokers;

public class UserBroker : BaseBroker, IUserService
{
    private static readonly HttpClient Client = new HttpClient();
    private const string baseUri = "http://user-service:80/UserService";

    public User? CreateProfile(User profile)
    {
        var uri = baseUri + "/CreateProfile";
        var content = new StringContent(JsonConvert.SerializeObject(profile), Encoding.UTF8, "application/json");
        var t = Post<User?>(uri, Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public User? GetProfileById(Guid id, bool withCV)
    {
        var uri = baseUri + "/GetProfileById/" + id;
        var t = Get<User?>(uri, Client);
        if (t != null) return t.Result;
        return null;
    }

    public User? UpdateProfile(User profile)
    {
        var uri = baseUri + "/UpdateProfile";
        var content = new StringContent(JsonConvert.SerializeObject(profile), Encoding.UTF8, "application/json");
        var t = Put<User?>(uri, Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public bool DeleteProfileById(Guid id)
    {
        var uri = baseUri + "/DeleteProfileById/" + id;
        var t = Delete<bool>(uri, Client);
        if (t != null) return t.Result;
        return false;
    }

    public User? ValidateProfile(LoginData loginData)
    {
        var uri = baseUri + "/ValidateProfile";
        var content = new StringContent(JsonConvert.SerializeObject(loginData), Encoding.UTF8, "application/json");
        var t = Post<User?>(uri, Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public bool ChangePassword(PasswordData passwordData)
    {
        var uri = baseUri + "/ChangePassword";
        var content = new StringContent(JsonConvert.SerializeObject(passwordData), Encoding.UTF8, "application/json");
        var t = Post<bool>(uri, Client, content);
        if (t != null) return t.Result;
        return false;
    }
}