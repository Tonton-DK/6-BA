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
