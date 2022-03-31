using System.Text;
using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Newtonsoft.Json;

namespace Frontend.Data_Brokers;

public class ContractBroker : BaseBroker, IContractService
{
    private static readonly HttpClient Client = new HttpClient();
    private const string Uri = "http://offer-service:80/ContractService";
    
    public Contract? Create(Contract contract)
    {
        var content = new StringContent(JsonConvert.SerializeObject(contract), Encoding.UTF8, "application/json");
        var t = Post<Contract>(Uri+"/Create", Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public Contract? Get(Guid id)
    {
        var t = Get<Contract>(Uri+"/Get/"+id, Client);
        if (t != null) return t.Result;
        return null;
    }

    public IEnumerable<Contract> List(Guid userId)
    {
        var t = Get<Contract[]>(Uri+"/List/"+userId, Client);
        if (t != null) return new List<Contract>(t.Result);
        return null;
    }

    public Contract? Update(Contract contract)
    {
        var content = new StringContent(JsonConvert.SerializeObject(contract), Encoding.UTF8, "application/json");
        var t = Put<Contract>(Uri+"/Update", Client, content);
        if (t != null) return t.Result;
        return null;
    }

    public bool Delete(Guid id)
    {
        var t = Delete<bool>(Uri+"/Delete/"+id, Client);
        if (t != null) return t.Result;
        return false;
    }
}