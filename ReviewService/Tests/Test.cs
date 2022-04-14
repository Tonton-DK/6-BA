using ClassLibrary.Classes;
using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using ReviewService.Controllers;
using ReviewService.Data_Providers;
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
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new ReviewServiceController(logger.Object, dataProvider);
        var output = service.CreateReview(input);
        
        Assert.AreSame(input, output);
    }
    
    [Test]
    public void GetReviewByIdTest()
    {
        var input = new Review(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "comment", 5, ReviewType.Provider);

        var logger = new Mock<ILogger<ReviewServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new ReviewServiceController(logger.Object, dataProvider);
        input = service.CreateReview(input);
        var output = service.GetReviewById(input.Id);
        
        Assert.AreEqual(input.Id, output.Id);
        Assert.AreEqual(input.ContractId, output.ContractId);
        Assert.AreEqual(input.CreatorId, output.CreatorId);
        Assert.AreEqual(input.TargetId, output.TargetId);
        Assert.AreEqual(input.Comment, output.Comment);
        Assert.AreEqual(input.Rating, output.Rating);
        Assert.AreEqual(input.Type, output.Type);
    }
    
    [Test]
    public void ListReviewsTest()
    {
        var input1 = new Review(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "comment", 1, ReviewType.Provider);
        var input2 = new Review(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), input1.TargetId, "comment", 3, ReviewType.Provider);
        var input3 = new Review(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "comment", 5, ReviewType.Provider);

        var logger = new Mock<ILogger<ReviewServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new ReviewServiceController(logger.Object, dataProvider);
        input1 = service.CreateReview(input1);
        input2 = service.CreateReview(input2);
        input3 = service.CreateReview(input3);
        
        var output = service.ListReviews(input1.TargetId, ReviewType.All);
        
        Assert.AreEqual(2, output.Count());
        Assert.AreEqual(true, output.Any(x => x.Id == input1.Id));
        Assert.AreEqual(true, output.Any(x => x.Id == input2.Id));
    }
    
    [Test]
    public void GetRatingTest()
    {
        var input1 = new Review(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "comment", 1, ReviewType.Provider);
        var input2 = new Review(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), input1.TargetId, "comment", 3, ReviewType.Provider);
        var input3 = new Review(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), input1.TargetId, "comment", 5, ReviewType.Provider);

        var logger = new Mock<ILogger<ReviewServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new ReviewServiceController(logger.Object, dataProvider);
        input1 = service.CreateReview(input1);
        input2 = service.CreateReview(input2);
        input3 = service.CreateReview(input3);
        
        var output = service.GetRating(input1.TargetId, ReviewType.All);
        
        Assert.AreEqual(3, output);
    }
    
    [Test]
    public void UpdateReviewTest()
    {
        var input = new Review(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "comment", 1, ReviewType.Provider);

        var logger = new Mock<ILogger<ReviewServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new ReviewServiceController(logger.Object, dataProvider);
        input = service.CreateReview(input);
        input.Comment = "new comment";
        input.Rating = 5;
        var output = service.UpdateReview(input);
        
        Assert.AreEqual(input, output);
    }
    
    [Test]
    public void DeleteReviewTest()
    {
        var input = new Review(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "comment", 1, ReviewType.Provider);

        var logger = new Mock<ILogger<ReviewServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new ReviewServiceController(logger.Object, dataProvider);
        input = service.CreateReview(input);
        var output1 = service.ListReviews(input.TargetId, ReviewType.All);
        var output2 = service.DeleteReview(input.Id);
        var output3 = service.ListReviews(input.TargetId, ReviewType.All);
        
        Assert.AreEqual(1, output1.Count());
        Assert.AreEqual(true, output2);
        Assert.AreEqual(0, output3.Count());
    }
}