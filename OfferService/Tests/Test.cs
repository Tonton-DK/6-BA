using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using Moq;
using NUnit.Framework;
using OfferService.Controllers;
using OfferService.Interfaces;

namespace OfferService.Tests;

public class Test
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void CreateOfferTest()
    {
        var input = new Offer(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);
        
        var logger = new Mock<ILogger<OfferServiceController>>();
        var dataProvider = new Mock<IDataProvider>();
        dataProvider.Setup(x => x.Create(input)).Returns(input);
        
        var jobService = new Mock<IJobService>();
        var contractService = new Mock<IContractService>();
        
        var service = new OfferServiceController(logger.Object, dataProvider.Object, jobService.Object, contractService.Object);
        var output = service.CreateOffer(input);
        
        Assert.AreSame(input, output);
    }
    
    [Test]
    public void AcceptOfferTest()
    {
        var input = new Offer(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), 500, "2 hours", DateTime.Now, State.Open);
        var job = new Job(input.JobId, "title", "description", DateTime.Now, new Category(Guid.NewGuid(), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid());

        var logger = new Mock<ILogger<OfferServiceController>>();
        var dataProvider = new Mock<IDataProvider>();
        dataProvider.Setup(x => x.AcceptOffer(input.Id)).Returns(input);
        
        var jobService = new Mock<IJobService>();
        jobService.Setup(x => x.GetJobById(input.JobId)).Returns(job);
        var contractService = new Mock<IContractService>();
        contractService.Setup(x => x.CreateContract(It.IsAny<Contract>())).Returns<Contract>(x => x);
        
        var service = new OfferServiceController(logger.Object, dataProvider.Object, jobService.Object, contractService.Object);
        var output = service.AcceptOffer(input.Id);

        Assert.AreEqual(job.Id, output.JobId);
        Assert.AreEqual(input.Id, output.OfferId);
        Assert.AreEqual(job.ClientId, output.ClientId);
        Assert.AreEqual(input.ProviderId, output.ProviderId);
        Assert.AreEqual(State.Open, output.ContractState);
    }
}