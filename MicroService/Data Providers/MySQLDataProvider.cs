using ClassLibrary;
using MicroService.Interfaces;
using MySql.Data.MySqlClient;

namespace MicroService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    public List<User> GetUsers()
    {
        string cs = @"server=database;userid=root;password=;database=test";

        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "INSERT INTO User(Firstname, Lastname) VALUES(@fn, @ln)";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@fn", "John");
        cmd.Parameters.AddWithValue("@ln", "Doe");
        cmd.Prepare();

        cmd.ExecuteNonQuery();

        Console.WriteLine("row inserted");

        var stm = "SELECT * FROM User";
        using var new_cmd = new MySqlCommand(stm, con);

        using MySqlDataReader rdr = new_cmd.ExecuteReader();

        var users = new List<User>();
        
        while (rdr.Read())
        {
            var user = new User(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2));
            users.Add(user);
        }

        return users;
    }
}