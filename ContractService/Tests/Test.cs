using ClassLibrary.Classes;
using ContractService.Controllers;
using ContractService.Interfaces;
using Moq;
using MySql.Data.MySqlClient;
using MySql.Server;
using NUnit.Framework;

namespace ContractService.Tests;

public class Test
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void DBTest()
    {
        MySqlServer database = MySqlServer.Instance;
        database.StartServer();

        var cs = database.GetConnectionString();
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "SELECT 1";
        using var cmd = new MySqlCommand(sql, con);
        
        var result = Convert.ToInt32(cmd.ExecuteScalar());
        
        database.ShutDown();
        Assert.AreEqual(1, result); 
        
        /*
        var sql = "UPDATE Contract SET Contract.JobId = @jobId, Contract.OfferId = @offerId, Contract.ClientId = @clientId, Contract.ProviderId = @providerId, Contract.CreationDate = @creationDate, Contract.ClosedDate = @closedDate, Contract.State = @state WHERE Contract.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        cmd.ExecuteNonQuery();
        */
    }
    
    [Test]
    public void CreateContractTest()
    {
        var input = new Contract(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, State.Open);
        
        var logger = new Mock<ILogger<ContractServiceController>>();
        var dataProvider = new Mock<IDataProvider>();
        dataProvider.Setup(x => x.Create(input)).Returns(input);
        
        var service = new ContractServiceController(logger.Object, dataProvider.Object);
        var output = service.CreateContract(input);
        
        Assert.AreSame(input, output);
    }
    
    [Test]
    public void ConcludeContractTest()
    {
        var ex = new Contract(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, State.Open);
        var input = new Contract(ex.Id, ex.JobId, ex.OfferId, ex.ClientId, ex.ProviderId, ex.CreationDate, ex.ContractState);

        var logger = new Mock<ILogger<ContractServiceController>>();
        var dataProvider = new Mock<IDataProvider>();
        dataProvider.Setup(x => x.Get(input.Id)).Returns(input);
        dataProvider.Setup(x => x.Update(It.IsAny<Contract>())).Returns<Contract>(x => x);
        
        var service = new ContractServiceController(logger.Object, dataProvider.Object);
        var output = service.ConcludeContract(input.Id);
        
        Assert.AreEqual(ex.Id, output.Id);
        Assert.AreEqual(ex.JobId, output.JobId);
        Assert.AreEqual(ex.OfferId, output.OfferId);
        Assert.AreEqual(ex.ClientId, output.ClientId);
        Assert.AreEqual(ex.ProviderId, output.ProviderId);
        Assert.AreEqual(ex.CreationDate, output.CreationDate);
        Assert.AreEqual(State.Concluded, output.ContractState);
        Assert.AreNotEqual(ex.ClosedDate, output.ClosedDate);
    }
    
    [Test]
    public void CancelContractTest()
    {
        var ex = new Contract(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), DateTime.Now, State.Open);
        var input = new Contract(ex.Id, ex.JobId, ex.OfferId, ex.ClientId, ex.ProviderId, ex.CreationDate, ex.ContractState);

        var logger = new Mock<ILogger<ContractServiceController>>();
        var dataProvider = new Mock<IDataProvider>();
        dataProvider.Setup(x => x.Get(input.Id)).Returns(input);
        dataProvider.Setup(x => x.Update(It.IsAny<Contract>())).Returns<Contract>(x => x);
        
        var service = new ContractServiceController(logger.Object, dataProvider.Object);
        var output = service.CancelContract(input.Id);
        
        Assert.AreEqual(ex.Id, output.Id);
        Assert.AreEqual(ex.JobId, output.JobId);
        Assert.AreEqual(ex.OfferId, output.OfferId);
        Assert.AreEqual(ex.ClientId, output.ClientId);
        Assert.AreEqual(ex.ProviderId, output.ProviderId);
        Assert.AreEqual(ex.CreationDate, output.CreationDate);
        Assert.AreEqual(State.Cancelled, output.ContractState);
        Assert.AreNotEqual(ex.ClosedDate, output.ClosedDate);
    }
}