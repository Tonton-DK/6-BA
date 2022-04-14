using ClassLibrary.Classes;
using JobService.Interfaces;
using MySql.Data.MySqlClient;

namespace JobService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    private string cs = @"server=job-database;userid=root;password=;database=db";
    public void setConnectionString(string connectionString) => cs = connectionString;

    # region Job
    public Job? CreateJob(Job job)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var sql = @"INSERT INTO Job(ID, Title, Description, Deadline, ClientID, CategoryID, Road, Number, Zip) 
                    VALUES(@id, @title, @description, @deadline, @client, @category, @road, @number, @zip)";
        using var cmd = new MySqlCommand(sql, con);

        job.Id = Guid.NewGuid();
        
        cmd.Parameters.AddWithValue("@id", job.Id);
        cmd.Parameters.AddWithValue("@title", job.Title);
        cmd.Parameters.AddWithValue("@description", job.Description);
        cmd.Parameters.AddWithValue("@deadline", job.Deadline.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd.Parameters.AddWithValue("@client", job.ClientId);
        cmd.Parameters.AddWithValue("@category", job.Category.Id);
        cmd.Parameters.AddWithValue("@road", job.Location.Road);
        cmd.Parameters.AddWithValue("@number", job.Location.Number);
        cmd.Parameters.AddWithValue("@zip", job.Location.Zip);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? job : null;
    }
    
    public Job? GetJob(Guid id)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var stm = @"SELECT Job.ID, Job.Title, Job.Description, Job.Deadline, Job.Road, Job.Number, Job.Zip, Job.ClientID, 
                        Job.CategoryID, Category.Name AS CategoryName, Category.Description AS CategoryDescription 
                    FROM Job join Category on Job.CategoryID = Category.ID WHERE Job.ID = @id";
        using var cmd = new MySqlCommand(stm, con);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();
        
        using MySqlDataReader rdr = cmd.ExecuteReader();
        
        if (rdr.Read())
        {
            var job = new Job(
                rdr.GetGuid(0),
                rdr.GetString(1), 
                rdr.GetString(2), 
                rdr.GetMySqlDateTime(3).Value,
                new Category(rdr.GetGuid(8), rdr.GetString(9), rdr.GetString(10)),
                new Address(rdr.GetString(4), rdr.GetString(5), rdr.GetString(6)),
                rdr.GetGuid(7));

            return job;
        }

        return null;
    }
    
    public List<Job> ListJobs(Filter filter)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var stm = @"SELECT Job.ID, Job.Title, Job.Description, Job.Deadline, Job.Road, Job.Number, Job.Zip, Job.ClientID, 
                        Job.CategoryID, Category.Name AS CategoryName, Category.Description AS CategoryDescription 
                    FROM Job join Category on Job.CategoryID = Category.ID";

        // Apply generic filter start
        if (filter.CategoryId != null ||
            filter.StartDate != null ||
            filter.EndDate != null ||
            filter.Zip.Length > 0 ||
            filter.SearchQuery.Length > 0)
        {
            stm += " WHERE 1=1 ";
        }
        
        if (filter.CategoryId != null)
            stm += "AND Job.CategoryID = @cat ";
        if (filter.StartDate != null)
            stm += "AND @from <= Job.Deadline ";
        if (filter.EndDate != null)
            stm += "AND Job.Deadline <= @to ";
        if (filter.Zip.Length > 0)
            stm += "AND Job.Zip = @zip ";
        if (filter.SearchQuery.Length > 0)
            stm += "AND (Job.Title LIKE @query OR Job.Description LIKE @query)";

        using var cmd = new MySqlCommand(stm, con);

        cmd.Parameters.AddWithValue("@cat", filter.CategoryId);
        cmd.Parameters.AddWithValue("@from", filter.StartDate?.Ticks);
        cmd.Parameters.AddWithValue("@to", filter.EndDate?.Ticks);
        cmd.Parameters.AddWithValue("@Zip", filter.Zip);
        cmd.Parameters.AddWithValue("@query", filter.SearchQuery);
        cmd.Prepare();
        
        using MySqlDataReader rdr = cmd.ExecuteReader();

        var jobs = new List<Job>();
        
        while (rdr.Read())
        {
            var job = new Job(
                rdr.GetGuid(0),
                rdr.GetString(1), 
                rdr.GetString(2), 
                rdr.GetDateTime(3),
                new Category(rdr.GetGuid(8), rdr.GetString(9), rdr.GetString(10)),
                new Address(rdr.GetString(4), rdr.GetString(5), rdr.GetString(6)),
                rdr.GetGuid(7));
            jobs.Add(job);
        }

        return jobs;
    }

    public IEnumerable<Job> ListJobsByUser(Guid userId)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var stm = @"SELECT Job.ID, Job.Title, Job.Description, Job.Deadline, Job.Road, Job.Number, Job.Zip, Job.ClientID, 
                        Job.CategoryID, Category.Name AS CategoryName, Category.Description AS CategoryDescription 
                    FROM Job join Category on Job.CategoryID = Category.ID
                    WHERE Job.ClientID = @clientID";
        
        using var cmd = new MySqlCommand(stm, con);

        cmd.Parameters.AddWithValue("@clientID", userId);
        cmd.Prepare();
        
        using MySqlDataReader rdr = cmd.ExecuteReader();

        var jobs = new List<Job>();
        
        while (rdr.Read())
        {
            var job = new Job(
                rdr.GetGuid(0),
                rdr.GetString(1), 
                rdr.GetString(2), 
                rdr.GetDateTime(3),
                new Category(rdr.GetGuid(8), rdr.GetString(9), rdr.GetString(10)),
                new Address(rdr.GetString(4), rdr.GetString(5), rdr.GetString(6)),
                rdr.GetGuid(7));
            jobs.Add(job);
        }

        return jobs;
    }

    public IEnumerable<Job> ListJobsByIDs(IEnumerable<Guid> jobIds)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var stm = @"SELECT Job.ID, Job.Title, Job.Description, Job.Deadline, Job.Road, Job.Number, Job.Zip, Job.ClientID, 
                        Job.CategoryID, Category.Name AS CategoryName, Category.Description AS CategoryDescription 
                    FROM Job join Category on Job.CategoryID = Category.ID
                    WHERE Job.ID in ('" + string.Join("','", jobIds) + "')";

        using var cmd = new MySqlCommand(stm, con);
        
        using MySqlDataReader rdr = cmd.ExecuteReader();

        var jobs = new List<Job>();
        
        while (rdr.Read())
        {
            var job = new Job(
                rdr.GetGuid(0),
                rdr.GetString(1), 
                rdr.GetString(2), 
                rdr.GetDateTime(3),
                new Category(rdr.GetGuid(8), rdr.GetString(9), rdr.GetString(10)),
                new Address(rdr.GetString(4), rdr.GetString(5), rdr.GetString(6)),
                rdr.GetGuid(7));
            jobs.Add(job);
        }

        return jobs;
    }

    public Job? UpdateJob(Job job)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var sql = @"UPDATE Job 
                    SET Title = @title, Description = @description, Deadline = @deadline, Road = @road, Number = @number, Zip = @zip
                    WHERE Job.ID = @id";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", job.Id);
        cmd.Parameters.AddWithValue("@title", job.Title);
        cmd.Parameters.AddWithValue("@description", job.Description);
        cmd.Parameters.AddWithValue("@deadline", job.Deadline.ToString("yyyy-MM-dd HH:mm:ss"));
        cmd.Parameters.AddWithValue("@road", job.Location.Road);
        cmd.Parameters.AddWithValue("@number", job.Location.Number);
        cmd.Parameters.AddWithValue("@zip", job.Location.Zip);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? job : null;
    }
    
    public bool DeleteJob(Guid id)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var sql = @"DELETE FROM Job WHERE Job.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? true : false;
    }
    # endregion

    # region Category
    public Category? CreateCategory(Category category)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var sql = @"INSERT INTO Category(ID, Name, Description) 
                    VALUES(@id, @name, @description)";
        using var cmd = new MySqlCommand(sql, con);

        category.Id = Guid.NewGuid();
        
        cmd.Parameters.AddWithValue("@id", category.Id);
        cmd.Parameters.AddWithValue("@name", category.Name);
        cmd.Parameters.AddWithValue("@description", category.Description);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? category : null;
    }
    
    public Category GetCategory(Guid id)
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var stm = @"SELECT Category.ID, Category.Name, Category.Description 
                    FROM Category WHERE Category.ID = @id";
        using var cmd = new MySqlCommand(stm, con);

        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();
        
        using MySqlDataReader rdr = cmd.ExecuteReader();
        
        if (rdr.Read())
        {
            var category = new Category(
                rdr.GetGuid(0),
                rdr.GetString(1), 
                rdr.GetString(2));

            return category;
        }

        return null;
    }
    
    public List<Category> ListCategories()
    {
        using var con = new MySqlConnection(cs);
        con.Open();
        
        var stm = @"SELECT Category.ID, Category.Name, Category.Description 
                    FROM Category";
        using var cmd = new MySqlCommand(stm, con);

        using MySqlDataReader rdr = cmd.ExecuteReader();

        var categories = new List<Category>();
        
        while (rdr.Read())
        {
            var category = new Category(
                rdr.GetGuid(0),
                rdr.GetString(1), 
                rdr.GetString(2));

            categories.Add(category);
        }

        return categories;
    }
    
    public Category UpdateCategory(Category category)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var sql = @"UPDATE Category 
                    SET Name = @name, Description = @description
                    WHERE Category.ID = @id";
        using var cmd = new MySqlCommand(sql, con);

        category.Id = Guid.NewGuid();
        
        cmd.Parameters.AddWithValue("@id", category.Id);
        cmd.Parameters.AddWithValue("@name", category.Name);
        cmd.Parameters.AddWithValue("@description", category.Description);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? category : null;
    }
    
    public bool DeleteCategory(Guid id)
    {
        using var con = new MySqlConnection(cs);
        con.Open();

        var sql = @"DELETE FROM Category WHERE Category.ID = @id";
        using var cmd = new MySqlCommand(sql, con);
        
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Prepare();

        var result = cmd.ExecuteNonQuery();
        return result > 0 ? true : false;
    }
    # endregion
}