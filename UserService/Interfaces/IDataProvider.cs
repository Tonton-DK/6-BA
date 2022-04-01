using ClassLibrary.Classes;

namespace UserService.Interfaces;

public interface IDataProvider
{
    public User? CreateUser(User user);
    public User? GetUserById(Guid id, bool withCV);
    public User? GetUserByLogin(string email, string password);
    public User? UpdateUser(User user);
    public bool DeleteUserById(Guid id);
    public bool ChangePassword(Guid userId, string oldPassword, string newPassword);
}