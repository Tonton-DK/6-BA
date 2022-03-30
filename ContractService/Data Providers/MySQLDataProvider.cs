using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using ContractService.Interfaces;

namespace ContractService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    public List<Contract> GetContracts()
    {
        string cs = @"server=contract-database;userid=root;password=;database=test";

        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "INSERT INTO Contract(ContractID, Firstname, Lastname) VALUES(@id, @fn, @ln)";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@fn", "John");
        cmd.Parameters.AddWithValue("@ln", "Doe");
        cmd.Prepare();

        cmd.ExecuteNonQuery();

        Console.WriteLine("row inserted");

        var stm = "SELECT * FROM Contract";
        using var new_cmd = new MySqlCommand(stm, con);

        using MySqlDataReader rdr = new_cmd.ExecuteReader();

        var contracts = new List<Contract>();
        
        while (rdr.Read())
        {
            var contract = new Contract(rdr.GetGuid(0), rdr.GetString(1), rdr.GetString(2));
            contracts.Add(contract);
        }

        return contracts;
    }
}