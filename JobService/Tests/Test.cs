using ClassLibrary.Classes;
using ClassLibrary.Interfaces;
using JobService.Controllers;
using JobService.Data_Providers;
using Moq;
using MySql.Data.MySqlClient;
using NUnit.Framework;
using ThrowawayDb.MySql;

namespace JobService.Tests;

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
        var sql = @"CREATE TABLE Category (
                      ID CHAR(36) PRIMARY KEY,
                      Name NVARCHAR(500) NOT NULL,
                      Description NVARCHAR(500) NOT NULL
                    );
                    INSERT INTO Category(ID, Name, Description) VALUES('156be3a6-5537-41f8-9608-705c7cd7cbc3', 'Garden Work', 'Fix my garden, please.');
                    INSERT INTO Category(ID, Name, Description) VALUES('0ebbe367-300a-4c86-9070-d6e106d7e4b9', 'Furniture Assembly', 'Fix my furniture, please.');
                    INSERT INTO Category(ID, Name, Description) VALUES('62507ed1-e247-4fb1-bbf8-0a73479cc911', 'Dog Walking', 'Walk my dog, please.');
                    CREATE TABLE Job (
                      ID CHAR(36) PRIMARY KEY,
                      Title NVARCHAR(500) NOT NULL,
                      Description NVARCHAR(500) NOT NULL,
                      Deadline DATETIME NOT NULL,
                      Road NVARCHAR(500) NOT NULL,
                      Number NVARCHAR(500) NOT NULL,
                      Zip NVARCHAR(500) NOT NULL,
                      ClientID CHAR(36) NOT NULL,
                      CategoryID CHAR(36),
                      FOREIGN KEY(CategoryID) REFERENCES Category(ID),
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
    public void ListCategoriesTest()
    {
        var expected = new Category(Guid.Parse("156be3a6-5537-41f8-9608-705c7cd7cbc3"), "Garden Work", "Fix my garden, please.");

        var logger = new Mock<ILogger<JobServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new JobServiceController(logger.Object, dataProvider);
        var output = service.ListCategories();
        
        Assert.AreEqual(3, output.Count());
        Assert.AreEqual(true, output.Any(x => x.Id == expected.Id));
        Assert.AreEqual(true, output.Any(x => x.Name == expected.Name));
        Assert.AreEqual(true, output.Any(x => x.Description == expected.Description));
    }
    
    [Test]
    public void CreateJobTest()
    {
        var input = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("156be3a6-5537-41f8-9608-705c7cd7cbc3"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Open);
        
        var logger = new Mock<ILogger<JobServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new JobServiceController(logger.Object, dataProvider);
        var output = service.CreateJob(input);
        
        Assert.AreSame(input, output);
    }
    
    [Test]
    public void GetJobByIdTest()
    {
        var input = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("156be3a6-5537-41f8-9608-705c7cd7cbc3"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Cancelled);
        
        var logger = new Mock<ILogger<JobServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new JobServiceController(logger.Object, dataProvider);
        input = service.CreateJob(input);
        var output = service.GetJobById(input.Id);
        
        Assert.AreEqual(input.Id, output.Id);
        Assert.AreEqual(input.Title, output.Title);
        Assert.AreEqual(input.Description, output.Description);
        Assert.AreEqual(input.Deadline.ToString("yyyy-mm-dd HH:MM"), output.Deadline.ToString("yyyy-mm-dd HH:MM"));
        Assert.AreEqual(input.Category.Id, output.Category.Id);
        Assert.AreEqual(input.Location.Road, output.Location.Road);
        Assert.AreEqual(input.Location.Number, output.Location.Number);
        Assert.AreEqual(input.Location.Zip, output.Location.Zip);
        Assert.AreEqual(input.ClientId, output.ClientId);
    }
    
    [Test]
    public void ListJobsTest()
    {
        var input1 = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("156be3a6-5537-41f8-9608-705c7cd7cbc3"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Open);
        var input2 = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("0ebbe367-300a-4c86-9070-d6e106d7e4b9"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Open);
        var input3 = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("62507ed1-e247-4fb1-bbf8-0a73479cc911"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Open);
        var input = new Filter(null, null, null, "", "");
        
        var logger = new Mock<ILogger<JobServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new JobServiceController(logger.Object, dataProvider);
        input1 = service.CreateJob(input1);
        input2 = service.CreateJob(input2);
        input3 = service.CreateJob(input3);
        
        var output = service.ListJobs(input).ToList();
        Assert.AreEqual(3, output.Count());
        Assert.AreEqual(true, output.Any(x => x.Id == input1.Id));
        Assert.AreEqual(true, output.Any(x => x.Id == input2.Id));
        Assert.AreEqual(true, output.Any(x => x.Id == input3.Id));
        
        input.CategoryId = Guid.Parse("156be3a6-5537-41f8-9608-705c7cd7cbc3");
        output = service.ListJobs(input).ToList();
        Assert.AreEqual(1, output.Count());
        Assert.AreEqual(input1.Id, output.First().Id);
    }
    
    [Test]
    public void ListJobsByUserTest()
    {
        var input1 = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("156be3a6-5537-41f8-9608-705c7cd7cbc3"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Open);
        var input2 = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("0ebbe367-300a-4c86-9070-d6e106d7e4b9"), "name", "description"), new Address("road", "2", "5000"), input1.ClientId, State.Open);
        var input3 = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("62507ed1-e247-4fb1-bbf8-0a73479cc911"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Open);
        
        var logger = new Mock<ILogger<JobServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new JobServiceController(logger.Object, dataProvider);
        input1 = service.CreateJob(input1);
        input2 = service.CreateJob(input2);
        input3 = service.CreateJob(input3);
        
        var output = service.ListJobsByUser(input1.ClientId).ToList();
        Assert.AreEqual(2, output.Count());
        Assert.AreEqual(true, output.Any(x => x.Id == input1.Id));
        Assert.AreEqual(true, output.Any(x => x.Id == input2.Id));
    }
    
    [Test]
    public void ListJobsByIDsTest()
    {
        var input1 = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("156be3a6-5537-41f8-9608-705c7cd7cbc3"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Open);
        var input2 = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("0ebbe367-300a-4c86-9070-d6e106d7e4b9"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Open);
        var input3 = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("62507ed1-e247-4fb1-bbf8-0a73479cc911"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Open);
        
        var logger = new Mock<ILogger<JobServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new JobServiceController(logger.Object, dataProvider);
        input1 = service.CreateJob(input1);
        input2 = service.CreateJob(input2);
        input3 = service.CreateJob(input3);
        
        var output = service.ListJobsByIDs(new []{input1.Id, input2.Id}).ToList();
        Assert.AreEqual(2, output.Count());
        Assert.AreEqual(true, output.Any(x => x.Id == input1.Id));
        Assert.AreEqual(true, output.Any(x => x.Id == input2.Id));
    }
    
    [Test]
    public void UpdateJobTest()
    {
        var input = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("156be3a6-5537-41f8-9608-705c7cd7cbc3"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Open);
        
        var logger = new Mock<ILogger<JobServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new JobServiceController(logger.Object, dataProvider);
        input = service.CreateJob(input);
        input.Title = "new title";
        input.Description = "new description";
        var output = service.UpdateJob(input);
        
        Assert.AreEqual(input, output);
    }
    
    [Test]
    public void DeleteJobByIdTest()
    {
        var input = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.Parse("156be3a6-5537-41f8-9608-705c7cd7cbc3"), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Open);
        
        var logger = new Mock<ILogger<JobServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new JobServiceController(logger.Object, dataProvider);
        input = service.CreateJob(input);
        var output1 = service.ListJobs(new Filter(null, null, null, "", ""));
        var output2 = service.DeleteJobById(input.Id);
        var output3 = service.ListJobs(new Filter(null, null, null, "", ""));
        
        Assert.AreEqual(1, output1.Count());
        Assert.AreEqual(true, output2);
        Assert.AreEqual(0, output3.Count());
    }
    
    [Test]
    public void CloseJobByIdTest()
    {
        var input = new Job(Guid.NewGuid(), "title", "description", DateTime.Now, new Category(Guid.NewGuid(), "name", "description"), new Address("road", "2", "5000"), Guid.NewGuid(), State.Open);

        var logger = new Mock<ILogger<JobServiceController>>();
        var dataProvider = new MySQLDataProvider();
        dataProvider.setConnectionString(database.ConnectionString);
        
        var service = new JobServiceController(logger.Object, dataProvider);
        input = service.CreateJob(input);
        var output = service.CloseJobById(input.Id);

        Assert.AreEqual(input.Id, output.Id);
        Assert.AreEqual(input.Title, output.Title);
        Assert.AreEqual(input.Description, output.Description);
        Assert.AreEqual(input.Deadline, output.Deadline);
        Assert.AreEqual(input.Category.Id, output.Category.Id);
        Assert.AreEqual(input.ClientId, output.ClientId);
        Assert.AreEqual(State.Open, output.JobState);
    }
}