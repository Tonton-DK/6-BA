using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using UserService.Interfaces;

namespace UserService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    private string cs = @"server=user-database;userid=root;password=;database=db";
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

    public User? CreateUser(User user, string salt, string hash)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;

        var sql =
            @"INSERT INTO User(ID, Email, PasswordSalt, PasswordHash, Firstname, Lastname, PhoneNumber, ProfilePicture, IsServiceProvider) 
                    VALUES(@id, @email, @passwordSalt, @passwordHash, @firstname, @lastname, @phoneNumber, @profilePicture, @isServiceProvider)";
        using var cmd = new MySqlCommand(sql, con);

        user.Id = Guid.NewGuid();
        cmd.Parameters.AddWithValue("@id", user.Id);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@passwordSalt", salt);
        cmd.Parameters.AddWithValue("@passwordHash", hash);
        cmd.Parameters.AddWithValue("@firstname", user.FirstName);
        cmd.Parameters.AddWithValue("@lastname", user.LastName);
        cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
        cmd.Parameters.AddWithValue("@profilePicture", user.ProfilePicture);
        cmd.Parameters.AddWithValue("@isServiceProvider", user.IsServiceProvider ? 1 : 0);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? user : null;
    }

    public User? GetUserById(Guid id)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;

        var stm = @"SELECT User.ID, User.Email, User.Firstname, User.Lastname, User.PhoneNumber, User.ProfilePicture, User.IsServiceProvider
                    FROM User
                    WHERE User.ID = @id";
        using var cmd = new MySqlCommand(stm, con);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            var user = new User(
                rdr.GetGuid(0),
                rdr.GetString(1),
                rdr.GetString(2),
                rdr.GetString(3),
                rdr.GetString(4),
                rdr.GetString(5),
                rdr.GetBoolean(6));
            return user;
        }

        return null;
    }

    public IEnumerable<User> ListUsersByIDs(IEnumerable<Guid> userIds)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return new List<User>();

        var stm = @"SELECT User.ID, User.Email, User.Firstname, User.Lastname, User.PhoneNumber, User.ProfilePicture, User.IsServiceProvider
                         FROM User
                         WHERE User.ID in ('" + string.Join("','", userIds) + "')";

        using var cmd = new MySqlCommand(stm, con);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        var users = new List<User>();

        while (rdr.Read())
        {
            var user = new User(
                rdr.GetGuid(0),
                rdr.GetString(1),
                rdr.GetString(2),
                rdr.GetString(3),
                rdr.GetString(4),
                rdr.GetString(5),
                rdr.GetBoolean(6));
            users.Add(user);
        }

        return users;
    }

    public UserValidator? GetUserValidator(string email)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;

        var stm =
            @"SELECT User.ID, User.Email, User.Firstname, User.Lastname, User.PhoneNumber, User.ProfilePicture, User.IsServiceProvider, User.PasswordSalt, User.PasswordHash
                    FROM User
                    WHERE User.Email = @email";
        using var cmd = new MySqlCommand(stm, con);

        cmd.Parameters.AddWithValue("@email", email);
        cmd.Prepare();

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            var user = new UserValidator(
                rdr.GetGuid(0),
                rdr.GetString(1),
                rdr.GetString(2),
                rdr.GetString(3),
                rdr.GetString(4),
                rdr.GetString(5),
                rdr.GetBoolean(6),
                rdr.GetString(7),
                rdr.GetString(8));

            return user;
        }

        return null;
    }

    public UserValidator? GetUserValidator(Guid id)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;

        var stm =
            @"SELECT User.ID, User.Email, User.Firstname, User.Lastname, User.PhoneNumber, User.ProfilePicture, User.IsServiceProvider, User.PasswordSalt, User.PasswordHash
                    FROM User
                    WHERE User.ID = @id";
        using var cmd = new MySqlCommand(stm, con);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();

        using MySqlDataReader rdr = cmd.ExecuteReader();

        if (rdr.Read())
        {
            var user = new UserValidator(
                rdr.GetGuid(0),
                rdr.GetString(1),
                rdr.GetString(2),
                rdr.GetString(3),
                rdr.GetString(4),
                rdr.GetString(5),
                rdr.GetBoolean(6),
                rdr.GetString(7),
                rdr.GetString(8));

            return user;
        }

        return null;
    }

    public User? UpdateUser(User user)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return null;

        var sql = @"UPDATE User 
                    SET Email = @email, Firstname = @firstName, Lastname = @lastName, PhoneNumber = @phoneNumber, ProfilePicture = @profilePicture, IsServiceProvider = @isServiceProvider
                    WHERE User.ID = @id";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", user.Id);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@firstname", user.FirstName);
        cmd.Parameters.AddWithValue("@lastname", user.LastName);
        cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
        cmd.Parameters.AddWithValue("@profilePicture", user.ProfilePicture);
        cmd.Parameters.AddWithValue("@isServiceProvider", user.IsServiceProvider ? 1 : 0);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? user : null;
    }

    public bool DeleteUserById(Guid id)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return false;

        var sql = @"DELETE FROM User WHERE User.ID = @id";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? true : false;
    }

    public bool ChangePassword(Guid userId, string newPasswordSalt, string newPasswordHash)
    {
        using var con = new MySqlConnection(cs);
        if (!Open(con))
            return false;

        var sql = @"UPDATE User 
                    SET PasswordSalt = @newPasswordSalt, PasswordHash = @newPasswordHash
                    WHERE User.ID = @id";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", userId);
        cmd.Parameters.AddWithValue("@newPasswordSalt", newPasswordSalt);
        cmd.Parameters.AddWithValue("@newPasswordHash", newPasswordHash);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? true : false;
    }
}