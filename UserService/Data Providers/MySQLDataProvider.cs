using ClassLibrary.Classes;
using MySql.Data.MySqlClient;
using UserService.Interfaces;

namespace UserService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    private static string cs = @"server=user-database;userid=root;password=;database=test";

    public User? CreateProfile(User user)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var sql = @"INSERT INTO User(ID, Email, Password, Firstname, Lastname, PhoneNumber, IsServiceProvider) 
                    VALUES(@id, @email, @password, @firstname, @lastname, @phoneNumber, @isServiceProvider)";
        using var cmd = new MySqlCommand(sql, con);

        user.Id = Guid.NewGuid();
        
        cmd.Parameters.AddWithValue("@id", user.Id);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@password", user.Password);
        cmd.Parameters.AddWithValue("@firstname", user.FirstName);
        cmd.Parameters.AddWithValue("@lastname", user.LastName);
        cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
        cmd.Parameters.AddWithValue("@isServiceProvider", user.IsServiceProvider ? 1 : 0);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();

        if (result > 0)
            return user;

        return null;
    }

    public User? GetProfileById(Guid id, bool withCV)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var stm = @"SELECT User.ID, User.Email, User.Password, User.Firstname, User.Lastname, User.PhoneNumber, User.IsServiceProvider
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

    public User? GetProfileByLogin(string email, string password)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var stm = @"SELECT User.ID, User.Email, User.Password, User.Firstname, User.Lastname, User.PhoneNumber, User.IsServiceProvider
                    FROM User
                    WHERE User.Email = @email AND User.Password = @password";
        using var cmd = new MySqlCommand(stm, con);

        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@password", password);
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

    public User? UpdateProfile(User user)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var sql = @"UPDATE User 
                    SET Email = @email, Firstname = @firstName, Lastname = @lastName, PhoneNumber = @phoneNumber, IsServiceProvider = @isServiceProvider
                    WHERE User.ID = @id";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", user.Id);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@password", user.Password);
        cmd.Parameters.AddWithValue("@firstname", user.FirstName);
        cmd.Parameters.AddWithValue("@lastname", user.LastName);
        cmd.Parameters.AddWithValue("@phoneNumber", user.PhoneNumber);
        cmd.Parameters.AddWithValue("@isServiceProvider", user.IsServiceProvider ? 1 : 0);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();

        if (result > 0)
            return user;

        return null;
    }

    public bool DeleteProfileById(Guid id)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var sql = @"DELETE FROM User WHERE User.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();

        if (result > 0)
            return true;

        return false;
    }

    public bool ChangePassword(Guid userId, string oldPassword, string newPassword)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var sql = @"UPDATE User 
                    SET Password = @newPassword
                    WHERE User.ID = @id AND User.Password = @oldPassword";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", userId);
        cmd.Parameters.AddWithValue("@oldPassword", oldPassword);
        cmd.Parameters.AddWithValue("@newPassword", newPassword);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();

        if (result > 0)
            return true;

        return false;
    }
}