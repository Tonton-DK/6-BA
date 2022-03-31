using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IContractService
{
    public Contract? CreateContract(Contract contract);
    public Contract? GetContractById(Guid id);
    public IEnumerable<Contract> ListContracts(Guid userId);
    public Contract? ConcludeContract(Guid id);
    public Contract? CancelContract(Guid id);
}