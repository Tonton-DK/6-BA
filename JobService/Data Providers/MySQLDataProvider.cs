using ClassLibrary;
using JobService.Interfaces;
using MySql.Data.MySqlClient;

namespace JobService.Data_Providers;

// Guide: https://zetcode.com/csharp/mysql/
public class MySQLDataProvider : IDataProvider
{
    public List<Job> GetJobs()
    {
        string cs = @"server=job-database;userid=root;password=;database=test";

        using var con = new MySqlConnection(cs);
        con.Open();
        
        var sql = "INSERT INTO Job(JobID, Title, Description) VALUES(@id, @ti, @de)";
        using var cmd = new MySqlCommand(sql, con);

        cmd.Parameters.AddWithValue("@id", Guid.NewGuid());
        cmd.Parameters.AddWithValue("@ti", "Title");
        cmd.Parameters.AddWithValue("@de", "Description");
        cmd.Prepare();

        cmd.ExecuteNonQuery();

        Console.WriteLine("row inserted");

        var stm = "SELECT * FROM Job";
        using var new_cmd = new MySqlCommand(stm, con);

        using MySqlDataReader rdr = new_cmd.ExecuteReader();

        var jobs = new List<Job>();
        
        while (rdr.Read())
        {
            var job = new Job(rdr.GetGuid(0), rdr.GetString(1), rdr.GetString(2));
            jobs.Add(job);
        }

        return jobs;
    }
}