using System.Text;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace ClassLibrary.Data_Brokers;

public class UserBroker : BaseBroker, IUserService
{
    private const string baseUri = "http://user-service:80/UserService";
    
    public bool Get()
    {
        var t = Get<bool>(baseUri+"/Get");
        if (t != null) return t.Result;
        return false;
    }

    public User? CreateUser(UserCreator user)
    {
        var uri = baseUri + "/CreateUser";
        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
        var t = Post<User?>(uri, content);
        if (t != null) return t.Result;
        return null;
    }

    public User? GetUserById(Guid id)
    {
        var uri = baseUri + "/GetUserById/" + id;
        var t = Get<User?>(uri);
        if (t != null) return t.Result;
        return null;
    }

    public User? UpdateUser(User user)
    {
        var uri = baseUri + "/UpdateUser";
        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
        var t = Put<User?>(uri, content);
        if (t != null) return t.Result;
        return null;
    }

    public bool DeleteUserById(Guid id)
    {
        var uri = baseUri + "/DeleteUserById/" + id;
        var t = Delete<bool>(uri);
        if (t != null) return t.Result;
        return false;
    }

    public User? ValidateUser(LoginRequest loginRequest)
    {
        var uri = baseUri + "/ValidateUser";
        var content = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");
        var t = Post<User?>(uri, content);
        if (t != null) return t.Result;
        return null;
    }

    public bool ChangePassword(PasswordRequest passwordRequest)
    {
        var uri = baseUri + "/ChangePassword";
        var content = new StringContent(JsonConvert.SerializeObject(passwordRequest), Encoding.UTF8, "application/json");
        var t = Post<bool>(uri, content);
        if (t != null) return t.Result;
        return false;
    }
}