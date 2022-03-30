using ClassLibrary.Classes;

namespace ContractService.Interfaces;

public interface IDataProvider
{
    public List<Contract> GetContracts();
}