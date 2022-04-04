using System.Text;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace Frontend.Data_Brokers;

public class ContractBroker : BaseBroker, IContractService
{
    private const string baseUri = "http://offer-service:80/ContractService";
    
    public bool Get()
    {
        var t = Get<bool>(baseUri+"/Get");
        if (t != null) return t.Result;
        return false;
    }
    
    public Contract? CreateContract(Contract contract)
    {
        var content = new StringContent(JsonConvert.SerializeObject(contract), Encoding.UTF8, "application/json");
        var t = Post<Contract>(baseUri+"/CreateContract", content);
        if (t != null) return t.Result;
        return null;
    }

    public Contract? GetContractById(Guid id)
    {
        var t = Get<Contract>(baseUri+"/GetContractById/"+id);
        if (t != null) return t.Result;
        return null;
    }

    public IEnumerable<Contract> ListContracts(Guid userId)
    {
        var t = Get<Contract[]>(baseUri+"/ListContracts/"+userId);
        if (t != null) return new List<Contract>(t.Result);
        return null;
    }

    public Contract? ConcludeContract(Guid id)
    {
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var t = Put<Contract>(baseUri+"/ConcludeContract/"+id, content);
        if (t != null) return t.Result;
        return null;
    }

    public Contract? CancelContract(Guid id)
    {
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var t = Put<Contract>(baseUri+"/CancelContract/"+id, content);
        if (t != null) return t.Result;
        return null;
    }
}