using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using ReviewService.Interfaces;

namespace ReviewService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    private string cs = @"server=review-database;userid=root;password=;database=db";
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

    public Review? Create(Review review)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;
        
        var sql = "INSERT INTO Review(ID, ContractId, CreatorId, TargetId, Comment, Rating, Type) VALUES(@id, @contractId, @creatorId, @targetId, @comment, @rating, @type)";
        using var cmd = new MySqlCommand(sql, con);

        review.Id = Guid.NewGuid();
        
        cmd.Parameters.AddWithValue("@id", review.Id);
        cmd.Parameters.AddWithValue("@contractId", review.ContractId);
        cmd.Parameters.AddWithValue("@creatorId", review.CreatorId);
        cmd.Parameters.AddWithValue("@targetId", review.TargetId);
        cmd.Parameters.AddWithValue("@comment", review.Comment);
        cmd.Parameters.AddWithValue("@rating", review.Rating);
        cmd.Parameters.AddWithValue("@type", review.Type.ToString());
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? review : null;
    }

    public Review? Get(Guid id)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;
        
        var sql = "SELECT * FROM Review WHERE Review.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            var review = new Review(
                rdr.GetGuid(0),
                rdr.GetGuid(1),
                rdr.GetGuid(2),
                rdr.GetGuid(3),
                rdr.GetString(4),
                rdr.GetInt32(5),
                (ReviewType) Enum.Parse(typeof(ReviewType), rdr.GetString(6))
            );

            return review;
        }

        return null;
    }

    public Review? Get(Guid contractId, Guid creatorId)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;
        
        var sql = "SELECT * FROM Review WHERE Review.ContractId = @contractId AND Review.CreatorId = @creatorId";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@contractId", contractId);
        cmd.Parameters.AddWithValue("@creatorId", creatorId);
        cmd.Prepare();

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            var review = new Review(
                rdr.GetGuid(0),
                rdr.GetGuid(1),
                rdr.GetGuid(2),
                rdr.GetGuid(3),
                rdr.GetString(4),
                rdr.GetInt32(5),
                (ReviewType) Enum.Parse(typeof(ReviewType), rdr.GetString(6))
            );

            return review;
        }

        return null;
    }

    public IEnumerable<Review> List(Guid userId)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return new List<Review>();
        
        var sql = "SELECT * FROM Review WHERE Review.TargetId = @userId";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@userId", userId);
        cmd.Prepare();

        using MySqlDataReader rdr = cmd.ExecuteReader();

        var reviews = new List<Review>();
        
        while (rdr.Read())
        {
            var review = new Review(
                rdr.GetGuid(0),
                rdr.GetGuid(1),
                rdr.GetGuid(2),
                rdr.GetGuid(3),
                rdr.GetString(4),
                rdr.GetInt32(5),
                (ReviewType) Enum.Parse(typeof(ReviewType), rdr.GetString(6))
            );

            reviews.Add(review);
        }

        return reviews;
    }

    public Review? Update(Review review)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;
        
        var sql = "UPDATE Review SET Review.Comment = @comment, Review.Rating = @rating WHERE Review.ID = @id";

        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", review.Id);
        cmd.Parameters.AddWithValue("@comment", review.Comment);
        cmd.Parameters.AddWithValue("@rating", review.Rating);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? review : null;
    }

    public bool Delete(Guid id)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return false;
        
        var sql = "DELETE FROM Review WHERE Review.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();
        
        var result = cmd.ExecuteNonQuery();
        return result > 0 ? true : false;
    }
}