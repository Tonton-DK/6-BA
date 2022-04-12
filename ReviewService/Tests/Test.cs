using ClassLibrary.Classes;
using Moq;
using NUnit.Framework;
using ReviewService.Controllers;
using ReviewService.Interfaces;

namespace ReviewService.Tests;

public class Test
{
    [SetUp]
    public void Setup()
    {
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