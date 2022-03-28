using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using OfferService.Interfaces;

namespace OfferService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    private static string cs = @"server=offer-database;userid=root;password=;database=test";

    public Offer? Create(Offer offer)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "INSERT INTO Offer(ID, JobId, ProviderId, PreviousOfferId, Price, Duration, Date) VALUES(@id, @jobId, @providerId, @previousOfferId, @price, @duration, @date)";
        using var cmd = new MySqlCommand(sql, con);

        offer.Id = Guid.NewGuid(); 
        
        cmd.Parameters.AddWithValue("@id", offer.Id);
        cmd.Parameters.AddWithValue("@jobId", offer.JobId);
        cmd.Parameters.AddWithValue("@providerId", offer.ProviderId);
        cmd.Parameters.AddWithValue("@previousOfferId", offer.PreviousOfferId ?? (object) DBNull.Value);
        cmd.Parameters.AddWithValue("@price", offer.Price);
        cmd.Parameters.AddWithValue("@duration", offer.Duration);
        cmd.Parameters.AddWithValue("@date", offer.Date);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();

        return result > 0 ? offer : null;
    }

    public Offer? Get(Guid id)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "SELECT * FROM Offer WHERE Offer.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", id);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
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

            return offer;
        }

        return null;
    }

    public List<Offer> List(Guid jobId)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "SELECT * FROM Offer WHERE Offer.JobId = @jobId";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@jobId", jobId);

        using MySqlDataReader rdr = cmd.ExecuteReader();

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

    public Offer? Update(Offer offer)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "UPDATE Offer SET Offer.JobId = @jobId, Offer.ProviderId = @providerId, Offer.PreviousOfferId = @previousOfferId, Offer.Price = @price, Offer.Duration = @duration, Offer.Date = @date WHERE Offer.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@jobId", offer.JobId);
        cmd.Parameters.AddWithValue("@providerId", offer.ProviderId);
        cmd.Parameters.AddWithValue("@previousOfferId", offer.PreviousOfferId ?? (object) DBNull.Value);
        cmd.Parameters.AddWithValue("@price", offer.Price);
        cmd.Parameters.AddWithValue("@duration", offer.Duration);
        cmd.Parameters.AddWithValue("@date", offer.Date);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();

        return result > 0 ? offer : null;
    }

    public bool Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}