using ClassLibrary.Classes;

namespace ContractService.Interfaces;

public interface IDataProvider
{
    public Contract? Create(Contract contract);
    public Contract? Get(Guid id);
    public List<Contract> List(Guid userId);
    public Contract? Update(Contract contract);
    public bool Delete(Guid id);
}