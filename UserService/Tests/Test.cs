using ClassLibrary.Classes;
using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using ThrowawayDb.MySql;
using UserService.Classes;
using UserService.Controllers;
using UserService.Data_Providers;
using UserService.Interfaces;

namespace UserService.Tests;

[TestFixture]
public class Test
{
    private ThrowawayDatabase database;
    
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        database = ThrowawayDatabase.Create(
            "root",
            "root",
            "localhost"
        );

        using var con = new MySqlConnection(database.ConnectionString);
        con.Open();
        var sql = @"CREATE TABLE User (
                      ID CHAR(36) PRIMARY KEY,
                      Email NVARCHAR(500) NOT NULL,
                      PasswordSalt NVARCHAR(500) NOT NULL,
                      PasswordHash NVARCHAR(500) NOT NULL,
                      Firstname NVARCHAR(500) NOT NULL,
                      Lastname NVARCHAR(500) NOT NULL,
                      PhoneNumber NVARCHAR(500) NOT NULL,
                      ProfilePicture NVARCHAR(500) NOT NULL,
                      IsServiceProvider BIT NOT NULL
                    );";
        using var cmd = new MySqlCommand(sql, con);
        cmd.ExecuteNonQuery();
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        database.Dispose();
    }
    
    [SetUp]
    public void Setup()
    {
        database.CreateSnapshot();
    }

    [TearDown]
    public void Cleanup()
    {
        database.RestoreSnapshot();
    }
    
    [Test]
    public void CreateUserTest()
    {
        var input = new UserCreator(Guid.NewGuid(), "email", "first name", "last name", "12345678", "urlPath", false, "password");

        var logger = new Mock<ILogger<UserServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);

        var service = new UserServiceController(logger.Object, dataProvider);
        var output = service.CreateUser(input);

        Assert.AreSame(input, output);
    }
    
    [Test]
    public void GetUserByIdTest()
    {
        var input = new UserCreator(Guid.NewGuid(), "email", "first name", "last name", "12345678", "urlPath", false, "password");

        var logger = new Mock<ILogger<UserServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);

        var service = new UserServiceController(logger.Object, dataProvider);
        input.Id = service.CreateUser(input).Id;
        var output = service.GetUserById(input.Id);
        
        Assert.AreEqual(input.Id, output.Id);
        Assert.AreEqual(input.Email, output.Email);
        Assert.AreEqual(input.FirstName, output.FirstName);
        Assert.AreEqual(input.LastName, output.LastName);
        Assert.AreEqual(input.PhoneNumber, output.PhoneNumber);
        Assert.AreEqual(input.IsServiceProvider, output.IsServiceProvider);
    }
    
    [Test]
    public void ListUsersByIDsTest()
    {
        var input1 = new UserCreator(Guid.NewGuid(), "email 1", "first name 1", "last name 1", "12345678", "urlPath",false, "password 1");
        var input2 = new UserCreator(Guid.NewGuid(), "email 2", "first name 2", "last name 2", "12345678", "urlPath", false, "password 2");
        var input3 = new UserCreator(Guid.NewGuid(), "email 3", "first name 3", "last name 3", "12345678", "urlPath", false, "password 3");

        var logger = new Mock<ILogger<UserServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);

        var service = new UserServiceController(logger.Object, dataProvider);
        input1.Id = service.CreateUser(input1).Id;
        input2.Id = service.CreateUser(input2).Id;
        input3.Id = service.CreateUser(input3).Id;
        var output = service.ListUsersByIDs(new []{input1.Id, input2.Id});
        
        Assert.AreEqual(2, output.Count());
        Assert.AreEqual(true, output.Any(x => x.Id == input1.Id));
        Assert.AreEqual(true, output.Any(x => x.Id == input2.Id));
    }
    
    [Test]
    public void UpdateUserTest()
    {
        var input = new UserCreator(Guid.NewGuid(), "email", "first name", "last name", "12345678", "urlPath", false, "password");

        var logger = new Mock<ILogger<UserServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);

        var service = new UserServiceController(logger.Object, dataProvider);
        input.Id = service.CreateUser(input).Id;
        input.Email = "new email";
        input.FirstName = "new first name";
        input.LastName = "new last name";
        var output = service.UpdateUser(input);
        
        Assert.AreEqual(input, output);
    }
    
    [Test]
    public void DeleteUserByIdTest()
    {
        var input = new UserCreator(Guid.NewGuid(), "email", "first name", "last name", "12345678", "urlPath", false, "password");

        var logger = new Mock<ILogger<UserServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);

        var service = new UserServiceController(logger.Object, dataProvider);
        input.Id = service.CreateUser(input).Id;
        var output1 = service.GetUserById(input.Id);
        var output2 = service.DeleteUserById(input.Id);
        var output3 = service.GetUserById(input.Id);
        
        Assert.AreEqual(input.Id, output1.Id);
        Assert.AreEqual(true, output2);
        Assert.AreEqual(null, output3);
    }
    
    [Test]
    public void ValidateUserTest()
    {
        var user = new UserCreator(Guid.NewGuid(), "email", "first name", "last name", "12345678", "urlPath", false, "password");
        var input = new LoginRequest(user.Email, user.Password);
        
        var logger = new Mock<ILogger<UserServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);

        var service = new UserServiceController(logger.Object, dataProvider);
        service.CreateUser(user);
        var output = service.ValidateUser(input);

        Assert.AreEqual(user.Id, output.Id);
        Assert.AreEqual(user.Email, output.Email);
        Assert.AreEqual(user.FirstName, output.FirstName);
        Assert.AreEqual(user.LastName, output.LastName);
        Assert.AreEqual(user.PhoneNumber, output.PhoneNumber);
        Assert.AreEqual(user.IsServiceProvider, output.IsServiceProvider);
    }
    
    [Test]
    public void ChangePasswordTest()
    {
        var user = new UserCreator(Guid.NewGuid(), "email", "first name", "last name", "12345678", "urlPath", false, "password");
        var input = new PasswordRequest(user.Id, user.Password, "more secret");
        
        var logger = new Mock<ILogger<UserServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);

        var service = new UserServiceController(logger.Object, dataProvider);
        service.CreateUser(user);
        input.UserId = user.Id;
        var output = service.ChangePassword(input);

        Assert.AreEqual(true, output);
    }
}
