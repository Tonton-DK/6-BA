using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using UserService.Interfaces;

namespace UserService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    private static string cs = @"server=user-database;userid=root;password=;database=db";

    public User? CreateUser(User user, string salt, string hash)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var sql =
            @"INSERT INTO User(ID, Email, PasswordSalt, PasswordHash, Firstname, Lastname, PhoneNumber, IsServiceProvider) 
                    VALUES(@id, @email, @passwordSalt, @passwordHash, @firstname, @lastname, @phoneNumber, @isServiceProvider)";
        using var cmd = new MySqlCommand(sql, con);

        user.Id = Guid.NewGuid();
        cmd.Parameters.AddWithValue("@id", user.Id);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@passwordSalt", salt);
        cmd.Parameters.AddWithValue("@passwordHash", hash);
        cmd.Parameters.AddWithValue("@firstname", user.FirstName);
        cmd.Parameters.AddWithValue("@lastname", user.LastName);
        cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
        cmd.Parameters.AddWithValue("@isServiceProvider", user.IsServiceProvider ? 1 : 0);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? user : null;
    }

    public User? GetUserById(Guid id)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var stm = @"SELECT User.ID, User.Email, User.Firstname, User.Lastname, User.PhoneNumber, User.IsServiceProvider
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
                rdr.GetBoolean(5));

            return user;
        }

        return null;
    }

    public UserValidator? GetUserValidator(string email)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var stm =
            @"SELECT User.ID, User.Email, User.Firstname, User.Lastname, User.PhoneNumber, User.IsServiceProvider, User.PasswordSalt, User.PasswordHash
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
                rdr.GetBoolean(5),
                rdr.GetString(6),
                rdr.GetString(7));

            return user;
        }

        return null;
    }

    public UserValidator? GetUserValidator(Guid id)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var stm =
            @"SELECT User.ID, User.Email, User.Firstname, User.Lastname, User.PhoneNumber, User.IsServiceProvider, User.PasswordSalt, User.PasswordHash
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
                rdr.GetBoolean(5),
                rdr.GetString(6),
                rdr.GetString(7));

            return user;
        }

        return null;
    }

    public User? UpdateUser(User user)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var sql = @"UPDATE User 
                    SET Email = @email, Firstname = @firstName, Lastname = @lastName, PhoneNumber = @phoneNumber, IsServiceProvider = @isServiceProvider
                    WHERE User.ID = @id";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", user.Id);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@firstname", user.FirstName);
        cmd.Parameters.AddWithValue("@lastname", user.LastName);
        cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
        cmd.Parameters.AddWithValue("@isServiceProvider", user.IsServiceProvider ? 1 : 0);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? user : null;
    }

    public bool DeleteUserById(Guid id)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

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
        con.Open();

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