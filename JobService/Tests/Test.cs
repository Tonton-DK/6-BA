using ClassLibrary.Classes;
using JobService.Controllers;
using JobService.Data_Providers;
using JobService.Interfaces;
using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using ThrowawayDb.MySql;

namespace JobService.Tests;

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
        var sql = @"CREATE TABLE Category (
                      ID CHAR(36) PRIMARY KEY,
                      Name NVARCHAR(500) NOT NULL,
                      Description NVARCHAR(500) NOT NULL
                    );
                    INSERT INTO Category(ID, Name, Description) VALUES('156be3a6-5537-41f8-9608-705c7cd7cbc3', 'Garden Work', 'Fix my garden, please.');
                    CREATE TABLE Job (
                      ID CHAR(36) PRIMARY KEY,
                      Title NVARCHAR(500) NOT NULL,
                      Description NVARCHAR(500) NOT NULL,
                      Deadline DATETIME NOT NULL,
                      Road NVARCHAR(500) NOT NULL,
                      Number NVARCHAR(500) NOT NULL,
                      Zip NVARCHAR(500) NOT NULL,
                      ClientID CHAR(36) NOT NULL,
                      CategoryID CHAR(36),
                      FOREIGN KEY(CategoryID) REFERENCES Category(ID)
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
    public void CreateJobTest()
    {
        var input = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("156be3a6-5537-41f8-9608-705c7cd7cbc3"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid());
        
        var logger = new Mock<ILogger<JobServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new JobServiceController(logger.Object, dataProvider);
        var output = service.CreateJob(input);
        
        Assert.AreSame(input, output);
    }
}