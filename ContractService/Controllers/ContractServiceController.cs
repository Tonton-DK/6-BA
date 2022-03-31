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

    [HttpPost("Create")]
    public Contract? Create([FromBody]Contract contract)
    {
        return _dataProvider.Create(contract);
    }

    [HttpGet("Get/{id}")]
    public Contract? Get(Guid id)
    {
        return _dataProvider.Get(id);
    }

    [HttpGet("List/{userId}")]
    public IEnumerable<Contract> List(Guid userId)
    {
        return _dataProvider.List(userId);
    }

    [HttpPut("Update")]
    public Contract? Update([FromBody]Contract contract)
    {
        return _dataProvider.Update(contract);
    }

    [HttpDelete("Delete/{id}")]
    public bool Delete(Guid id)
    {
        return _dataProvider.Delete(id);
    }
}
