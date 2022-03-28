using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using OfferService.Interfaces;

namespace OfferService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    public List<Offer> GetOffers(Guid JobId)
    {
        string cs = @"server=offer-database;userid=root;password=;database=test";

        using var con = new MySqlConnection(cs);
        con.Open();
        
        // Insert test data
        var sql = "INSERT INTO Offer(OfferId, ParentJobId, ProviderId, Price, Duration, Date) VALUES(@OfferId, @ParentJobId, @ProviderId, @Price, @Duration, @Date)";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@OfferId", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@ParentJobId", JobId);
        cmd.Parameters.AddWithValue("@ProviderId", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@Price", "400");
        cmd.Parameters.AddWithValue("@Duration", "2 Hours");
        cmd.Parameters.AddWithValue("@Date", DateTime.Today);
        cmd.Prepare();

        cmd.ExecuteNonQuery();

        Console.WriteLine("row inserted");

        // Get test data
        var stm = "SELECT * FROM Offer WHERE ParentJobId = @JobId";
        using var new_cmd = new MySqlCommand(stm, con);
        
        new_cmd.Parameters.AddWithValue("@JobId", JobId);

        using MySqlDataReader rdr = new_cmd.ExecuteReader();

        var offers = new List<Offer>();
        
        while (rdr.Read())
        {
            var offer = new Offer(
                rdr.GetGuid(0),
                rdr.GetGuid(1),
                rdr.GetGuid(2),
                rdr.GetInt32(4), 
                rdr.GetString(5), 
                rdr.GetDateTime(6));
            
            if(!rdr.IsDBNull(3))
            {
                offer.PreviousOfferId = rdr.GetGuid(3);
            }
            
            offers.Add(offer);
        }

        return offers;
    }

    public Offer GetOffer(int OfferId)
    {
        throw new NotImplementedException();
    }
}