using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using ContractService.Interfaces;

namespace ContractService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    private static string cs = @"server=contract-database;userid=root;password=;database=db";

    public Contract? Create(Contract contract)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "INSERT INTO Contract(ID, JobId, OfferId, ClientId, ProviderId, CreationDate, ClosedDate, State) VALUES(@id, @jobId, @offerId, @clientId, @providerId, @creationDate, @closedDate, @state)";
        using var cmd = new MySqlCommand(sql, con);
        
        contract.Id = Guid.NewGuid();
        
        cmd.Parameters.AddWithValue("@id", contract.Id);
        cmd.Parameters.AddWithValue("@jobId", contract.JobId);
        cmd.Parameters.AddWithValue("@offerId", contract.OfferId);
        cmd.Parameters.AddWithValue("@clientId", contract.ClientId);
        cmd.Parameters.AddWithValue("@providerId", contract.ProviderId);
        cmd.Parameters.AddWithValue("@creationDate", contract.CreationDate);
        cmd.Parameters.AddWithValue("@closedDate", contract.ClosedDate ?? (object) DBNull.Value);
        cmd.Parameters.AddWithValue("@state", contract.ContractState.ToString());
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? contract : null;
    }

    public Contract? Get(Guid id)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "SELECT * FROM Contract WHERE Contract.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            var contract = new Contract(
                rdr.GetGuid(0),
                rdr.GetGuid(1),
                rdr.GetGuid(2),
                rdr. GetGuid(3),
                rdr.GetGuid(4),
                rdr.GetDateTime(5),
                (State) Enum.Parse(typeof(State), rdr.GetString(7))
            );
            
            if(!rdr.IsDBNull(6))
            {
                contract.ClosedDate = rdr.GetDateTime(6);
            }

            return contract;
        }

        return null;
    }

    public List<Contract> List(Guid userId)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "SELECT * FROM Contract WHERE Contract.ClientId = @userId OR Contract.ProviderId = @userId";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@userId", userId);
        cmd.Prepare();

        using MySqlDataReader rdr = cmd.ExecuteReader();

        var contracts = new List<Contract>();
        
        while (rdr.Read())
        {
            var contract = new Contract(
                rdr.GetGuid(0),
                rdr.GetGuid(1),
                rdr.GetGuid(2),
                rdr. GetGuid(3),
                rdr.GetGuid(4),
                rdr.GetDateTime(5),
                (State) Enum.Parse(typeof(State), rdr.GetString(7))
            );
            
            if(!rdr.IsDBNull(6))
            {
                contract.ClosedDate = rdr.GetDateTime(6);
            }
            
            contracts.Add(contract);
        }

        return contracts;
    }

    public Contract? Update(Contract contract)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "UPDATE Contract SET Contract.JobId = @jobId, Contract.OfferId = @offerId, Contract.ClientId = @clientId, Contract.ProviderId = @providerId, Contract.CreationDate = @creationDate, Contract.ClosedDate = @closedDate, Contract.State = @state WHERE Contract.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", contract.Id);
        cmd.Parameters.AddWithValue("@jobId", contract.JobId);
        cmd.Parameters.AddWithValue("@offerId", contract.OfferId);
        cmd.Parameters.AddWithValue("@clientId", contract.ClientId);
        cmd.Parameters.AddWithValue("@providerId", contract.ProviderId);
        cmd.Parameters.AddWithValue("@creationDate", contract.CreationDate);
        cmd.Parameters.AddWithValue("@closedDate", contract.ClosedDate ?? (object) DBNull.Value);
        cmd.Parameters.AddWithValue("@state", contract.ContractState.ToString());
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? contract : null;
    }

    public bool Delete(Guid id)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "DELETE * FROM Contract WHERE Contract.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();
        
        var result = cmd.ExecuteNonQuery();
        return result > 0 ? true : false;
    }
}