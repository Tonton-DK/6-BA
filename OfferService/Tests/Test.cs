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
    public void GetOfferByIdTest()
    {
        var input = new Offer(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);
        
        var logger = new Mock<ILogger<OfferServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var jobService = new Mock<IJobService>();
        var contractService = new Mock<IContractService>();
        
        var service = new OfferServiceController(logger.Object, dataProvider, jobService.Object, contractService.Object);
        input = service.CreateOffer(input);
        var output = service.GetOfferById(input.Id);
        
        Assert.AreEqual(input.Id, output.Id);
        Assert.AreEqual(input.JobId, output.JobId);
        Assert.AreEqual(input.ProviderId, output.ProviderId);
        Assert.AreEqual(input.PreviousOfferId, output.PreviousOfferId);
        Assert.AreEqual(input.Price, output.Price);
        Assert.AreEqual(input.Duration, output.Duration);
        Assert.AreEqual(input.Date.ToString("yyyy-mm-dd HH:MM"), output.Date.ToString("yyyy-mm-dd HH:MM"));
        Assert.AreEqual(input.State, output.State);
    }

    [Test]
    public void ListOffersForJobTest()
    {
        var input1 = new Offer(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);
        var input2 = new Offer(Guid.NewGuid(), input1.JobId, Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);
        var input3 = new Offer(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);
        
        var logger = new Mock<ILogger<OfferServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var jobService = new Mock<IJobService>();
        var contractService = new Mock<IContractService>();
        
        var service = new OfferServiceController(logger.Object, dataProvider, jobService.Object, contractService.Object);
        input1 = service.CreateOffer(input1);
        input2 = service.CreateOffer(input2);
        input3 = service.CreateOffer(input3);
        
        var output = service.ListOffersForJob(input1.JobId);
        
        Assert.AreEqual(2, output.Count());
        Assert.AreEqual(true, output.Any(x => x.Id == input1.Id));
        Assert.AreEqual(true, output.Any(x => x.Id == input2.Id));
    }

    [Test]
    public void ListOffersForUserTest()
    {
        var input1 = new Offer(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);
        var input2 = new Offer(Guid.NewGuid(), Guid.NewGuid(), input1.ProviderId, 500, "2 hours", DateTime.Now, State.Open);
        var input3 = new Offer(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);
        
        var logger = new Mock<ILogger<OfferServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var jobService = new Mock<IJobService>();
        var contractService = new Mock<IContractService>();
        
        var service = new OfferServiceController(logger.Object, dataProvider, jobService.Object, contractService.Object);
        input1 = service.CreateOffer(input1);
        input2 = service.CreateOffer(input2);
        input3 = service.CreateOffer(input3);
        
        var output = service.ListOffersForUser(input1.ProviderId);
        
        Assert.AreEqual(2, output.Count());
        Assert.AreEqual(true, output.Any(x => x.Id == input1.Id));
        Assert.AreEqual(true, output.Any(x => x.Id == input2.Id));
    }

    [Test]
    public void UpdateOfferTest()
    {
        var input = new Offer(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);
        
        var logger = new Mock<ILogger<OfferServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var jobService = new Mock<IJobService>();
        var contractService = new Mock<IContractService>();
        
        var service = new OfferServiceController(logger.Object, dataProvider, jobService.Object, contractService.Object);
        input = service.CreateOffer(input);
        input.Price = 400;
        input.Duration = "1.5 hours";
        var output = service.UpdateOffer(input);
        
        Assert.AreEqual(input, output);
    }
    
    [Test]
    public void DeleteOfferTest()
    {
        var input = new Offer(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);
        
        var logger = new Mock<ILogger<OfferServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var jobService = new Mock<IJobService>();
        var contractService = new Mock<IContractService>();
        
        var service = new OfferServiceController(logger.Object, dataProvider, jobService.Object, contractService.Object);
        input = service.CreateOffer(input);
        var output1 = service.ListOffersForUser(input.ProviderId);
        var output2 = service.DeleteOffer(input.Id);
        var output3 = service.ListOffersForUser(input.ProviderId);
        
        Assert.AreEqual(1, output1.Count());
        Assert.AreEqual(true, output2);
        Assert.AreEqual(0, output3.Count());
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
    
    [Test]
    public void CreateCounterOfferTest()
    {
        var job = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.NewGuid(), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid());
        var offer = new Offer(Guid.NewGuid(), job.Id, Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);
        var input = new Offer(Guid.NewGuid(), job.Id, offer.ProviderId, 400, "1.5 hours", DateTime.Now, State.Open);
        
        var logger = new Mock<ILogger<OfferServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var jobService = new Mock<IJobService>();
        jobService.Setup(x => x.GetJobById(offer.JobId)).Returns(job);
        var contractService = new Mock<IContractService>();
        
        var service = new OfferServiceController(logger.Object, dataProvider, jobService.Object, contractService.Object);
        offer = service.CreateOffer(offer);
        var output = service.CreateCounterOffer(offer.Id, input);

        Assert.AreEqual(job.Id, output.JobId);
        Assert.AreEqual(offer.Id, output.PreviousOfferId);
        Assert.AreEqual(offer.ProviderId, output.ProviderId);
        Assert.AreNotEqual(offer.Price, output.Price);
        Assert.AreNotEqual(offer.Duration, output.Duration);
        Assert.AreEqual(offer.Date.ToString("yyyy-MM-dd HH:mm:ss"), output.Date.ToString("yyyy-MM-dd HH:mm:ss"));
        Assert.AreEqual(offer.State, output.State);
    }
    
    [Test]
    public void DeclineOfferTest()
    {
        var job = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.NewGuid(), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid());
        var input = new Offer(Guid.NewGuid(), job.Id, Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);

        var logger = new Mock<ILogger<OfferServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var jobService = new Mock<IJobService>();
        jobService.Setup(x => x.GetJobById(input.JobId)).Returns(job);
        var contractService = new Mock<IContractService>();
        
        var service = new OfferServiceController(logger.Object, dataProvider, jobService.Object, contractService.Object);
        input = service.CreateOffer(input);
        var output1 = service.DeclineOffer(input.Id);
        var output2 = service.GetOfferById(input.Id);

        Assert.AreEqual(true, output1);
        Assert.AreEqual(State.Cancelled, output2.State);
    }
}