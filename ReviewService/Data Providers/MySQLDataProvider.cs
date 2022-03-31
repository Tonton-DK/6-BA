using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using ReviewService.Interfaces;

namespace ReviewService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    public List<Review> GetReviews()
    {
        string cs = @"server=user-database;userid=root;password=;database=test";

        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "INSERT INTO Review(UserID, Firstname, Lastname) VALUES(@id, @fn, @ln)";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@fn", "John");
        cmd.Parameters.AddWithValue("@ln", "Doe");
        cmd.Prepare();

        cmd.ExecuteNonQuery();

        Console.WriteLine("row inserted");

        var stm = "SELECT * FROM Review";
        using var new_cmd = new MySqlCommand(stm, con);

        using MySqlDataReader rdr = new_cmd.ExecuteReader();

        var users = new List<Review>();
        
        while (rdr.Read())
        {
            var user = new Review(rdr.GetGuid(0), rdr.GetString(1), rdr.GetString(2));
            users.Add(user);
        }

        return users;
    }

    public Review? Create(Review review)
    {
        throw new NotImplementedException();
    }

    public Review? Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Review> List(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Review? Update(Review review)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}