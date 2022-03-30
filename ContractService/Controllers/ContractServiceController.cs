using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ContractService.Interfaces;

namespace ContractService.Controllers;

[ApiController]
[Route("[controller]")]
public class ContractServiceController : ControllerBase, IContractService
{
    private readonly ILogger<ContractServiceController> _logger;
    private readonly IDataProvider _dataProvider;

    public ContractServiceController(ILogger<ContractServiceController> logger, IDataProvider dataProvider)
    {
        _logger = logger;
        _dataProvider = dataProvider;
    }

    [HttpGet]
    public IEnumerable<Contract> Get()
    {
        var contracts = _dataProvider.GetContracts();
        return contracts.ToArray();
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
