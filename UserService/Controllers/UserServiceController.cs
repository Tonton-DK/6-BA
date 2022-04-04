using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
    public User? CreateUser([FromBody] User user)
    {
        return _dataProvider.CreateUser(user);
    }

    [HttpGet("GetUserById/{id}")]
    public User? GetUserById(Guid id, bool withCV)
    {
        return _dataProvider.GetUserById(id, withCV);
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
    public User? ValidateUser([FromBody] LoginData loginData)
    {
        return _dataProvider.GetUserByLogin(loginData.Email, loginData.Password);
    }

    [HttpPost("ChangePassword")]
    public bool ChangePassword([FromBody] PasswordData passwordData)
    {
        return _dataProvider.ChangePassword(passwordData.UserId, passwordData.OldPassword, passwordData.NewPassword);
    }
}
