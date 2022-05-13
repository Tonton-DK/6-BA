using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using ContractService.Interfaces;

namespace ContractService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    private string cs = @"server=contract-database;userid=root;password=;database=db";
    public void setConnectionString(string connectionString) => cs = connectionString;
    private readonly ILogger<MySQLDataProvider>? _logger;

    public MySQLDataProvider(ILogger<MySQLDataProvider>? logger = null)
    {
        _logger = logger;
    }

    private bool Open(MySqlConnection con)
    {
        try
        {
            con.Open();
            return true;
        }
        catch (Exception err)
        {
            _logger?.LogError(err, "Failed to connect to DB");
            return false;
        }
    }
    
    public Contract? Create(Contract contract)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;

        var sql = "INSERT INTO Contract(ID, JobId, OfferId, ClientId, ProviderId, CreationDate, ClosedDate, State) VALUES(@id, @jobId, @offerId, @clientId, @providerId, @creationDate, @closedDate, @state)";
        using var cmd = new MySqlCommand(sql, con);

        contract.Id = Guid.NewGuid();

        cmd.Parameters.AddWithValue("@id", contract.Id);
        cmd.Parameters.AddWithValue("@jobId", contract.JobId);
        cmd.Parameters.AddWithValue("@offerId", contract.OfferId);
        cmd.Parameters.AddWithValue("@clientId", contract.ClientId);
        cmd.Parameters.AddWithValue("@providerId", contract.ProviderId);
        cmd.Parameters.AddWithValue("@creationDate", contract.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd.Parameters.AddWithValue("@closedDate", contract.ClosedDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object) DBNull.Value);
        cmd.Parameters.AddWithValue("@state", contract.ContractState.ToString());
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? contract : null;
    }

    public Contract? Get(Guid id)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;

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
                rdr.GetGuid(3),
                rdr.GetGuid(4),
                rdr.GetMySqlDateTime(5).Value,
                (State) Enum.Parse(typeof(State), rdr.GetString(7))
            );

            if (!rdr.IsDBNull(6))
            {
                contract.ClosedDate = rdr.GetMySqlDateTime(6).Value;
            }

            return contract;
        }

        return null;
    }

    public IEnumerable<Contract> List(Guid userId)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return new List<Contract>();

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
                rdr.GetGuid(3),
                rdr.GetGuid(4),
                rdr.GetMySqlDateTime(5).Value,
                (State) Enum.Parse(typeof(State), rdr.GetString(7))
            );

            if (!rdr.IsDBNull(6))
            {
                contract.ClosedDate = rdr.GetMySqlDateTime(6).Value;
            }

            contracts.Add(contract);
        }

        return contracts;
    }

    public Contract? Update(Contract contract)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;

        var sql =
            "UPDATE Contract SET Contract.JobId = @jobId, Contract.OfferId = @offerId, Contract.ClientId = @clientId, Contract.ProviderId = @providerId, Contract.CreationDate = @creationDate, Contract.ClosedDate = @closedDate, Contract.State = @state WHERE Contract.ID = @id";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", contract.Id);
        cmd.Parameters.AddWithValue("@jobId", contract.JobId);
        cmd.Parameters.AddWithValue("@offerId", contract.OfferId);
        cmd.Parameters.AddWithValue("@clientId", contract.ClientId);
        cmd.Parameters.AddWithValue("@providerId", contract.ProviderId);
        cmd.Parameters.AddWithValue("@creationDate", contract.CreationDate.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd.Parameters.AddWithValue("@closedDate", contract.ClosedDate?.ToString("yyyy-MM-dd HH:mm:ss") ?? (object) DBNull.Value);
        cmd.Parameters.AddWithValue("@state", contract.ContractState.ToString());
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? contract : null;
    }

    public bool Delete(Guid id)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return false;

        var sql = "DELETE FROM Contract WHERE Contract.ID = @id";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? true : false;
    }
}