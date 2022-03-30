using ClassLibrary.Classes;

namespace ClassLibrary.Interfaces;

public interface IContractService
{
    public Contract? Create(Contract contract);
    public Contract? Get(Guid id);
    public IEnumerable<Contract> List(Guid userId);
    public Contract? Update(Contract contract);
    public bool Delete(Guid id);
}