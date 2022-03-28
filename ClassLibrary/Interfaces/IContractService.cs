using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IContractService
{
    public IEnumerable<Contract> Get();
}