using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using OfferService.Interfaces;

namespace OfferService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    private string cs = @"server=offer-database;userid=root;password=;database=db";
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

    public Offer? Create(Offer offer)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;
        
        var sql = "INSERT INTO Offer(ID, JobId, ProviderId, PreviousOfferId, Price, Duration, Date, State, Comment) VALUES(@id, @jobId, @providerId, @previousOfferId, @price, @duration, @date, @state, @comment)";
        using var cmd = new MySqlCommand(sql, con);

        offer.Id = Guid.NewGuid(); 
        
        cmd.Parameters.AddWithValue("@id", offer.Id);
        cmd.Parameters.AddWithValue("@jobId", offer.JobId);
        cmd.Parameters.AddWithValue("@providerId", offer.ProviderId);
        cmd.Parameters.AddWithValue("@previousOfferId", offer.PreviousOfferId ?? (object) DBNull.Value);
        cmd.Parameters.AddWithValue("@price", offer.Price);
        cmd.Parameters.AddWithValue("@duration", offer.Duration);
        cmd.Parameters.AddWithValue("@date", offer.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd.Parameters.AddWithValue("@state", offer.State.ToString());
        cmd.Parameters.AddWithValue("@comment", offer.Comment);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? offer : null;
    }

    public Offer? Get(Guid id)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;
        
        var sql = "SELECT * FROM Offer WHERE Offer.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            var offer = new Offer(
                rdr.GetGuid(0),
                rdr.GetGuid(1),
                rdr.GetGuid(2),
                rdr.GetInt32(4), 
                rdr.GetString(5), 
                rdr.GetMySqlDateTime(6).Value,
                (State) Enum.Parse(typeof(State), rdr.GetString(7)),
                rdr.GetString(8)
                );
            
            if(!rdr.IsDBNull(3))
            {
                offer.PreviousOfferId = rdr.GetGuid(3);
            }

            return offer;
        }

        return null;
    }

    public IEnumerable<Offer> ListForJob(Guid jobId)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return new List<Offer>();
        
        var sql = "SELECT * FROM Offer WHERE Offer.JobId = @jobId";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@jobId", jobId);
        cmd.Prepare();

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
                rdr.GetMySqlDateTime(6).Value,
                (State) Enum.Parse(typeof(State), rdr.GetString(7)),
                rdr.GetString(8)
                );
            
            if(!rdr.IsDBNull(3))
            {
                offer.PreviousOfferId = rdr.GetGuid(3);
            }
            
            offers.Add(offer);
        }

        return offers;
    }

    public IEnumerable<Offer> ListForUser(Guid userId)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return new List<Offer>();
        
        var sql = "SELECT * FROM Offer WHERE Offer.ProviderId = @userId";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@userId", userId);
        cmd.Prepare();

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
                rdr.GetMySqlDateTime(6).Value,
                (State) Enum.Parse(typeof(State), rdr.GetString(7)),
                rdr.GetString(8)
            );
            
            if(!rdr.IsDBNull(3))
            {
                offer.PreviousOfferId = rdr.GetGuid(3);
            }
            
            offers.Add(offer);
        }

        return offers;
    }

    public IEnumerable<Offer> ListForIDs(IEnumerable<Guid> offerIds)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return new List<Offer>();

        var stm = @"SELECT * FROM Offer WHERE Offer.ID in ('" + string.Join("','", offerIds) + "')";

        using var cmd = new MySqlCommand(stm, con);

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
                rdr.GetMySqlDateTime(6).Value,
                (State) Enum.Parse(typeof(State), rdr.GetString(7)),
                rdr.GetString(8)
            );
            
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
        if (!Open(con))
            return null;
        
        var sql = "UPDATE Offer SET Offer.Price = @price, Offer.Duration = @duration, Offer.Date = @date, Offer.State = @state, Offer.Comment = @comment WHERE Offer.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", offer.Id);
        cmd.Parameters.AddWithValue("@price", offer.Price);
        cmd.Parameters.AddWithValue("@duration", offer.Duration);
        cmd.Parameters.AddWithValue("@date", offer.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd.Parameters.AddWithValue("@state", offer.State.ToString());
        cmd.Parameters.AddWithValue("@comment", offer.Comment);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? offer : null;
    }

    public bool Delete(Guid id)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return false;
        
        var sql = "DELETE FROM Offer WHERE Offer.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();
        
        var result = cmd.ExecuteNonQuery();
        return result > 0 ? true : false;
    }

    public Offer? AcceptOffer(Guid id)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;
        
        var sql = "UPDATE Offer SET Offer.State = @state WHERE Offer.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@state", State.Concluded.ToString());
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? Get(id) : null;
    }

    public bool DeclineOffer(Guid id)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return false;
        
        var sql = "UPDATE Offer SET Offer.State = @state WHERE Offer.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@state", State.Cancelled.ToString());
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? true : false;
    }
}