using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UserService.Classes;
using UserService.Interfaces;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class UserServiceController : ControllerBase, IUserService
{
    private readonly ILogger<UserServiceController> _logger;
    private readonly IDataProvider _dataProvider;

    public UserServiceController(ILogger<UserServiceController> logger, IDataProvider dataProvider)
    {
        _logger = logger;
        _dataProvider = dataProvider;
    }

    [HttpGet("Get")]
    public bool Get()
    {
        return true;
    }

    [HttpPost("CreateUser")]
    public User? CreateUser([FromBody] UserCreator user)
    {
        var salt = Salt.Create();
        var hash = Hash.Create(user.Password, salt);
        return _dataProvider.CreateUser(user, salt, hash);
    }

    [HttpGet("GetUserById/{id}")]
    public User? GetUserById(Guid id)
    {
        return _dataProvider.GetUserById(id);
    }

    [HttpPost("ListUsersByIDs")]
    public IEnumerable<User> ListUsersByIDs([FromBody] IEnumerable<Guid> userIds)
    {
        return _dataProvider.ListUsersByIDs(userIds);
    }

    [HttpPut("UpdateUser")]
    public User? UpdateUser([FromBody] User user)
    {
        return _dataProvider.UpdateUser(user);
    }

    [HttpDelete("DeleteUserById/{id}")]
    public bool DeleteUserById(Guid id)
    {
        return _dataProvider.DeleteUserById(id);
    }

    [HttpPost("ValidateUser")]
    public User? ValidateUser([FromBody] LoginRequest loginRequest)
    {
        var user = _dataProvider.GetUserValidator(loginRequest.Email);
        if (user != null)
        {
            var result = Hash.Validate(loginRequest.Password, user.PasswordSalt, user.PasswordHash);
            if (result)
            {
                return user;
            }
        }

        return null;
    }

    [HttpPost("ChangePassword")]
    public bool ChangePassword([FromBody] PasswordRequest passwordRequest)
    {
        var user = _dataProvider.GetUserValidator(passwordRequest.UserId);
        if (user != null)
        {
            var result = Hash.Validate(passwordRequest.OldPassword, user.PasswordSalt, user.PasswordHash);
            if (result)
            {
                var salt = Salt.Create();
                var hash = Hash.Create(passwordRequest.NewPassword, salt);
                return _dataProvider.ChangePassword(passwordRequest.UserId, salt, hash);
            }

            return false;
        }

        return false;
    }
}