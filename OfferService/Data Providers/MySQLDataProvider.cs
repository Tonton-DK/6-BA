using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using OfferService.Interfaces;

namespace OfferService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    private static string cs = @"server=offer-database;userid=root;password=;database=test";

    public Offer Create(Offer offer)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "INSERT INTO Offer(OfferId, ParentJobId, ProviderId, PreviousOfferId, Price, Duration, Date) VALUES(@OfferId, @ParentJobId, @ProviderId, @PreviousOfferId, @Price, @Duration, @Date)";
        using var cmd = new MySqlCommand(sql, con);

        Guid id = Guid.NewGuid();
        offer.Id = id; 
        
        cmd.Parameters.AddWithValue("@OfferId", offer.Id);
        cmd.Parameters.AddWithValue("@ParentJobId", offer.JobId);
        cmd.Parameters.AddWithValue("@ProviderId", offer.ProviderId);
        cmd.Parameters.AddWithValue("@PreviousOfferId", offer.PreviousOfferId ?? (object) DBNull.Value);
        cmd.Parameters.AddWithValue("@Price", offer.Price);
        cmd.Parameters.AddWithValue("@Duration", offer.Duration);
        cmd.Parameters.AddWithValue("@Date", offer.Date);
        cmd.Prepare();

        cmd.ExecuteNonQuery();

        return offer;
    }

    public Offer Get(Guid id)
    {
        throw new NotImplementedException();
    }

    public List<Offer> List(Guid jobId)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var stm = "SELECT * FROM Offer WHERE ParentJobId = @JobId";
        using var new_cmd = new MySqlCommand(stm, con);
        
        new_cmd.Parameters.AddWithValue("@JobId", jobId);

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

    public Offer Update(Offer offer)
    {
        throw new NotImplementedException();
    }

    public bool Delete(Guid id)
    {
        throw new NotImplementedException();
    }
}