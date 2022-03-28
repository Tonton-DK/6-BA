using ClassLibrary;
using MySql.Data.MySqlClient;
using UserService.Interfaces;

namespace UserService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    public List<Offer> GetOffers()
    {
        string cs = @"server=offer-database;userid=root;password=;database=test";

        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "INSERT INTO User(Firstname, Lastname) VALUES(@fn, @ln)";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@fn", "John");
        cmd.Parameters.AddWithValue("@ln", "Doe");
        cmd.Prepare();

        cmd.ExecuteNonQuery();

        Console.WriteLine("row inserted");

        var stm = "SELECT * FROM Offer";
        using var new_cmd = new MySqlCommand(stm, con);

        using MySqlDataReader rdr = new_cmd.ExecuteReader();

        var offers = new List<Offer>();
        
        while (rdr.Read())
        {
            var offer = new Offer(
                rdr.GetInt32(0),
                rdr.GetInt32(0),
                rdr.GetInt32(0),
                rdr.GetInt32(0), 
                rdr.GetString(1), 
                rdr.GetDateTime(2));
            offers.Add(offer);
        }

        return offers;
    }

    public Offer GetOffer(int OfferId)
    {
        throw new NotImplementedException();
    }
}