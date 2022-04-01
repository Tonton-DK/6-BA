using ClassLibrary.Classes;

namespace UserService.Interfaces;

public interface IDataProvider
{
    public User? CreateProfile(User profile);
    public User? GetProfileById(Guid id, bool withCV);
    public User? GetProfileByLogin(string email, string password);
    public User? UpdateProfile(User profile);
    public bool DeleteProfileById(Guid id);
    public bool ChangePassword(Guid userId, string oldPassword, string newPassword);
}