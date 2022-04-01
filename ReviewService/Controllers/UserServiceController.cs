using ClassLibrary.Classes;
using Microsoft.AspNetCore.Mvc;
using UserService.Interfaces;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class UserServiceController// : ControllerBase, IUserService
{
    private readonly ILogger<UserServiceController> _logger;
    private readonly IDataProvider _dataProvider;

    public UserServiceController(ILogger<UserServiceController> logger, IDataProvider dataProvider)
    {
        _logger = logger;
        _dataProvider = dataProvider;
    }

    [HttpGet]
    public IEnumerable<User> Get()
    {
        var users = _dataProvider.GetUsers();
        return users.ToArray();
    }
}
