using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UserService.Interfaces;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class ContractServiceController : ControllerBase, IUserService
{
    private readonly ILogger<ContractServiceController> _logger;
    private readonly IDataProvider _dataProvider;

    public ContractServiceController(ILogger<ContractServiceController> logger, IDataProvider dataProvider)
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
