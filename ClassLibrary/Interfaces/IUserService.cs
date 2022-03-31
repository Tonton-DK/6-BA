using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IUserService
{
    public User? CreateProfile(User profile);
    public User? GetProfileById(Guid id, bool withCV);
    public User? UpdateProfile(User profile);
    public bool DeleteProfileById(Guid id);
    public bool ValidateProfile(string email, string password);
    public bool ChangePassword(Guid id, string oldPassword, string newPassword);
}