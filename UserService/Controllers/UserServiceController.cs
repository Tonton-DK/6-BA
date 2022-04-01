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

    [HttpPost("CreateProfile")]
    public User? CreateProfile([FromBody] User profile)
    {
        return _dataProvider.CreateProfile(profile);
    }

    [HttpGet("GetProfileById/{id}")]
    public User? GetProfileById(Guid id, bool withCV)
    {
        return _dataProvider.GetProfileById(id, withCV);
    }

    [HttpPut("UpdateProfile")]
    public User? UpdateProfile([FromBody] User profile)
    {
        return _dataProvider.UpdateProfile(profile);
    }

    [HttpDelete("DeleteProfileById/{id}")]
    public bool DeleteProfileById(Guid id)
    {
        return _dataProvider.DeleteProfileById(id);
    }

    [HttpPost("ValidateProfile")]
    public User? ValidateProfile([FromBody] LoginData loginData)
    {
        return _dataProvider.GetProfileByLogin(loginData.Email, loginData.Password);
    }

    [HttpPost("ChangePassword")]
    public bool ChangePassword([FromBody] PasswordData passwordData)
    {
        return _dataProvider.ChangePassword(passwordData.UserId, passwordData.OldPassword, passwordData.NewPassword);
    }
}
