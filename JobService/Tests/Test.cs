using ClassLibrary.Classes;
using JobService.Controllers;
using JobService.Interfaces;
using Moq;
using NUnit.Framework;

namespace JobService.Tests;

public class Test
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void CreateJobTest()
    {
        var input = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.NewGuid(), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid());
        
        var logger = new Mock<ILogger<JobServiceController>>();
        var dataProvider = new Mock<IDataProvider>();
        dataProvider.Setup(x => x.CreateJob(input)).Returns(input);
        
        var service = new JobServiceController(logger.Object, dataProvider.Object);
        var output = service.CreateJob(input);
        
        Assert.AreSame(input, output);
    }
}