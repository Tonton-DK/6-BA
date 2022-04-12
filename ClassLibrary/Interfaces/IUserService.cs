using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IUserService : IBaseService
{
    public User? CreateUser(UserCreator user);
    public User? GetUserById(Guid id);
    public User? UpdateUser(User user);
    public bool DeleteUserById(Guid id);
    public User? ValidateUser(LoginRequest loginRequest);
    public bool ChangePassword(PasswordRequest passwordRequest);
}