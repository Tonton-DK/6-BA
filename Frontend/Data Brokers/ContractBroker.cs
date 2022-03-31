using System.Text;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace Frontend.Data_Brokers;

public class ContractBroker : BaseBroker, IContractService
{
    private static readonly HttpClient Client = new HttpClient();
    private const string Uri = "http://offer-service:80/ContractService";
    
    public Contract? CreateContract(Contract contract)
    {
        var content = new StringContent(JsonConvert.SerializeObject(contract), Encoding.UTF8, "application/json");
        var t = Post<Contract>(Uri+"/CreateContract", Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public Contract? GetContractById(Guid id)
    {
        var t = Get<Contract>(Uri+"/GetContractById/"+id, Client);
        if (t != null) return t.Result;
        return null;
    }

    public IEnumerable<Contract> ListContracts(Guid userId)
    {
        var t = Get<Contract[]>(Uri+"/ListContracts/"+userId, Client);
        if (t != null) return new List<Contract>(t.Result);
        return null;
    }

    public Contract? ConcludeContract(Guid id)
    {
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var t = Put<Contract>(Uri+"/ConcludeContract/"+id, Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public Contract? CancelContract(Guid id)
    {
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var t = Put<Contract>(Uri+"/CancelContract/"+id, Client, content);
        if (t != null) return t.Result;
        return null;
    }
}