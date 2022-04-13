using ClassLibrary.Classes;
using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using ReviewService.Controllers;
using ReviewService.Interfaces;
using ThrowawayDb.MySql;

namespace ReviewService.Tests;

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
        var sql = @"CREATE TABLE Review (
                      ID CHAR(36) PRIMARY KEY,
                      ContractId CHAR(36) NOT NULL,
                      CreatorId CHAR(36) NOT NULL,
                      TargetId CHAR(36) NOT NULL,
                      Comment VARCHAR(500) NOT NULL,
                      Rating SMALLINT NOT NULL,
                      Type ENUM('Client', 'Provider') NOT NULL
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
    public void CreateReviewTest()
    {
        var input = new Review(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "comment", 5, ReviewType.Provider);

        var logger = new Mock<ILogger<ReviewServiceController>>();
        var dataProvider = new Mock<IDataProvider>();
        dataProvider.Setup(x => x.Create(input)).Returns(input);
        
        var service = new ReviewServiceController(logger.Object, dataProvider.Object);
        var output = service.CreateReview(input);
        
        Assert.AreSame(input, output);
    }
}