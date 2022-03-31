using ClassLibrary.Classes;
using ClassLibrary.Interfaces;

namespace Frontend.Data_Brokers;

public class UserBroker : BaseBroker, IUserService
{
    private static readonly HttpClient Client = new HttpClient();
    private const string Uri = "http://user-service:80/UserService";

    public User? CreateProfile(User profile)
    {
        throw new NotImplementedException();
    }

    public User? GetProfileById(Guid id, bool withCV)
    {
        throw new NotImplementedException();
    }

    public User? UpdateProfile(User profile)
    {
        throw new NotImplementedException();
    }

    public bool DeleteProfileById(Guid id)
    {
        throw new NotImplementedException();
    }

    public bool ValidateProfile(string email, string password)
    {
        throw new NotImplementedException();
    }

    public bool ChangePassword(Guid id, string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }
}