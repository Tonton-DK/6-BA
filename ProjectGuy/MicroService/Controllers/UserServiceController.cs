using ClassLibrary;
using MicroService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.Controllers;

[ApiController]
[Route("[controller]")]
public class UserServiceController : ControllerBase
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

    [HttpGet("GetByName/{name}")]
    public IActionResult GetByName(string name)
    {
        return Ok("Name: " + name);
    }

    [HttpGet("GetById/{id}")]
    public IActionResult GetById(Guid id)
    {
        return Ok("Id: " + id);
    } 
}
