using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using UserService.Interfaces;

namespace UserService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    public List<User> GetUsers()
    {
        string cs = @"server=user-database;userid=root;password=;database=test";

        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "INSERT INTO User(UserID, Firstname, Lastname) VALUES(@id, @fn, @ln)";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
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
            var user = new User(rdr.GetGuid(0), "","","","","",false);
            users.Add(user);
        }

        return users;
    }
}