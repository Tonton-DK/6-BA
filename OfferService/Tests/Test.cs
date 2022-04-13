using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using OfferService.Controllers;
using OfferService.Data_Providers;
using OfferService.Interfaces;
using ThrowawayDb.MySql;

namespace OfferService.Tests;

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
        var sql = @"CREATE TABLE Offer (
                      ID CHAR(36) PRIMARY KEY,
                      JobId CHAR(36) NOT NULL,
                      ProviderId CHAR(36) NOT NULL,
                      PreviousOfferId CHAR(36),
                      Price int NOT NULL,
                      Duration VARCHAR(500) NOT NULL,
                      DATE DATETIME NOT NULL,
                      State ENUM('Open', 'Concluded', 'Cancelled')
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
    public void CreateOfferTest()
    {
        var input = new Offer(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);
        
        var logger = new Mock<ILogger<OfferServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var jobService = new Mock<IJobService>();
        var contractService = new Mock<IContractService>();
        
        var service = new OfferServiceController(logger.Object, dataProvider, jobService.Object, contractService.Object);
        var output = service.CreateOffer(input);
        
        Assert.AreSame(input, output);
    }
    
    [Test]
    public void AcceptOfferTest()
    {
        var job = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.NewGuid(), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid());
        var input = new Offer(Guid.NewGuid(), job.Id, Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);

        var logger = new Mock<ILogger<OfferServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var jobService = new Mock<IJobService>();
        jobService.Setup(x => x.GetJobById(input.JobId)).Returns(job);
        var contractService = new Mock<IContractService>();
        contractService.Setup(x => x.CreateContract(It.IsAny<Contract>())).Returns<Contract>(x => x);
        
        var service = new OfferServiceController(logger.Object, dataProvider, jobService.Object, contractService.Object);
        input = service.CreateOffer(input);
        var output = service.AcceptOffer(input.Id);

        Assert.AreEqual(job.Id, output.JobId);
        Assert.AreEqual(input.Id, output.OfferId);
        Assert.AreEqual(job.ClientId, output.ClientId);
        Assert.AreEqual(input.ProviderId, output.ProviderId);
        Assert.AreEqual(State.Open, output.ContractState);
    }
}