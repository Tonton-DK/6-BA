using ClassLibrary;
using MySql.Data.MySqlClient;
using OfferService.Interfaces;

namespace OfferService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    public List<Offer> GetOffers()
    {
        string cs = @"server=offer-database;userid=root;password=;database=test";

        using var con = new MySqlConnection(cs);
        con.Open();
        
        // Insert test data
        var sql = "INSERT INTO Offer(OfferId, JobId, ProviderId, Price, Duration, Date) VALUES(@OfferId, @JobId, @ProviderId, @Price, @Duration, @Date)";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@OfferId", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@JobId", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@ProviderId", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@Price", "400");
        cmd.Parameters.AddWithValue("@Duration", "2 Hours");
        cmd.Parameters.AddWithValue("@Date", DateTime.Today);
        cmd.Prepare();

        cmd.ExecuteNonQuery();

        Console.WriteLine("row inserted");

        // Get test data
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