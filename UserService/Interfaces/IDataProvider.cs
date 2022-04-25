using ClassLibrary.Classes;

namespace UserService.Interfaces;

public interface IDataProvider
{
    public User? CreateUser(User user, string salt, string hash);
    public User? GetUserById(Guid id);
    IEnumerable<User> ListUsersByIDs(IEnumerable<Guid> userIds);
    public UserValidator? GetUserValidator(string email);
    public UserValidator? GetUserValidator(Guid id);
    public User? UpdateUser(User user);
    public bool DeleteUserById(Guid id);
    public bool ChangePassword(Guid userId, string newPasswordSalt, string newPasswordHash);
}