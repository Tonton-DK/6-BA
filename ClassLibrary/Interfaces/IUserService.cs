using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IUserService : IBaseService
{
    public User? CreateUser(User user);
    public User? GetUserById(Guid id, bool withCV);
    public User? UpdateUser(User user);
    public bool DeleteUserById(Guid id);
    public User? ValidateUser(LoginData loginData);
    public bool ChangePassword(PasswordData passwordData);
}