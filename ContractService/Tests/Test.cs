using ClassLibrary.Classes;
using ContractService.Controllers;
using ContractService.Data_Providers;
using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using ThrowawayDb.MySql;

namespace ContractService.Tests;

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
        var sql = @"CREATE TABLE Contract (
                      ID CHAR(36) PRIMARY KEY,
                      JobId CHAR(36) NOT NULL,
                      OfferId CHAR(36) NOT NULL,
                      ClientId CHAR(36) NOT NULL,
                      ProviderId CHAR(36) NOT NULL,
                      CreationDate DATETIME NOT NULL,
                      ClosedDate DATETIME,
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
    public void CreateContractTest()
    {
        var input = new Contract(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, State.Open);
        
        var logger = new Mock<ILogger<ContractServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new ContractServiceController(logger.Object, dataProvider);
        var output = service.CreateContract(input);
        
        Assert.AreSame(input, output);
    }
    
    [Test]
    public void ConcludeContractTest()
    {
        var ex = new Contract(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, State.Open);
        var input = new Contract(ex.Id, ex.JobId, ex.OfferId, ex.ClientId, ex.ProviderId, ex.CreationDate, ex.ContractState);

        var logger = new Mock<ILogger<ContractServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new ContractServiceController(logger.Object, dataProvider);
        input = service.CreateContract(input);
        ex.Id = input.Id;
        var output = service.ConcludeContract(input.Id);
        
        Assert.AreEqual(ex.Id, output.Id);
        Assert.AreEqual(ex.JobId, output.JobId);
        Assert.AreEqual(ex.OfferId, output.OfferId);
        Assert.AreEqual(ex.ClientId, output.ClientId);
        Assert.AreEqual(ex.ProviderId, output.ProviderId);
        Assert.AreEqual(ex.CreationDate.ToString("yyyy-mm-dd HH:MM"), output.CreationDate.ToString("yyyy-mm-dd HH:MM"));
        Assert.AreEqual(State.Concluded, output.ContractState);
        Assert.AreNotEqual(ex.ClosedDate, output.ClosedDate);
    }
    
    [Test]
    public void CancelContractTest()
    {
        var ex = new Contract(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, State.Open);
        var input = new Contract(ex.Id, ex.JobId, ex.OfferId, ex.ClientId, ex.ProviderId, ex.CreationDate, ex.ContractState);

        var logger = new Mock<ILogger<ContractServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new ContractServiceController(logger.Object, dataProvider);
        input = service.CreateContract(input);
        ex.Id = input.Id;
        var output = service.CancelContract(input.Id);
        
        Assert.AreEqual(ex.Id, output.Id);
        Assert.AreEqual(ex.JobId, output.JobId);
        Assert.AreEqual(ex.OfferId, output.OfferId);
        Assert.AreEqual(ex.ClientId, output.ClientId);
        Assert.AreEqual(ex.ProviderId, output.ProviderId);
        Assert.AreEqual(ex.CreationDate.ToString("yyyy-mm-dd HH:MM"), output.CreationDate.ToString("yyyy-mm-dd HH:MM"));
        Assert.AreEqual(State.Cancelled, output.ContractState);
        Assert.AreNotEqual(ex.ClosedDate, output.ClosedDate);
    }
}