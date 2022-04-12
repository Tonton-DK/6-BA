using ClassLibrary.Classes;
using Moq;
using NUnit.Framework;
using UserService.Classes;
using UserService.Controllers;
using UserService.Interfaces;

namespace UserService.Tests;

public class Test
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void CreateUserTest()
    {
        var input = new UserCreator(Guid.NewGuid(), "email", "first name", "last name", "12345678", false, "password");

        var logger = new Mock<ILogger<UserServiceController>>();
        var dataProvider = new Mock<IDataProvider>();
        dataProvider.Setup(x => x.CreateUser(input, It.IsAny<string>(), It.IsAny<string>())).Returns(input);

        var service = new UserServiceController(logger.Object, dataProvider.Object);
        var output = service.CreateUser(input);

        Assert.AreSame(input, output);
    }

    [Test]
    public void ValidateUserTest()
    {
        var user = new UserCreator(Guid.NewGuid(), "email", "first name", "last name", "12345678", false, "password");
        var input = new LoginRequest(user.Email, user.Password);

        var salt = Salt.Create();
        var hash = Hash.Create(input.Password, salt);
        var validator = new UserValidator(user.Id, user.Email, user.FirstName, user.LastName, user.PhoneNumber, user.IsServiceProvider, salt, hash);

        var logger = new Mock<ILogger<UserServiceController>>();
        var dataProvider = new Mock<IDataProvider>();
        dataProvider.Setup(x => x.GetUserValidator(input.Email)).Returns(validator);

        var service = new UserServiceController(logger.Object, dataProvider.Object);
        var output = service.ValidateUser(input);

        Assert.AreEqual(user.Id, output.Id);
        Assert.AreEqual(user.Email, output.Email);
        Assert.AreEqual(user.FirstName, output.FirstName);
        Assert.AreEqual(user.LastName, output.LastName);
        Assert.AreEqual(user.PhoneNumber, output.PhoneNumber);
        Assert.AreEqual(user.IsServiceProvider, output.IsServiceProvider);
    }
    
    [Test]
    public void ChangePasswordTest()
    {
        var user = new UserCreator(Guid.NewGuid(), "email", "first name", "last name", "12345678", false, "password");
        var input = new PasswordRequest(user.Id, user.Password, "more secret");

        var salt = Salt.Create();
        var hash = Hash.Create(input.OldPassword, salt);
        var validator = new UserValidator(user.Id, user.Email, user.FirstName, user.LastName, user.PhoneNumber, user.IsServiceProvider, salt, hash);

        var logger = new Mock<ILogger<UserServiceController>>();
        var dataProvider = new Mock<IDataProvider>();
        dataProvider.Setup(x => x.GetUserValidator(input.UserId)).Returns(validator);
        dataProvider.Setup(x => x.ChangePassword(user.Id, It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var service = new UserServiceController(logger.Object, dataProvider.Object);
        var output = service.ChangePassword(input);

        Assert.AreEqual(true, output);
    }
}