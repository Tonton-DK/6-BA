namespace ClassLibrary.Classes;

public class Job
{
    public Job(Guid id, string title, string description, DateTime deadline, Category category, Address location, Guid clientId, State jobState)
    {
        Id = id;
        Title = title;
        Description = description;
        Deadline = deadline;
        Category = category;
        Location = location;
        ClientId = clientId;
        JobState = jobState;
    }

    public Job()
    {
        Id = Guid.Empty;
        Title = String.Empty;
        Description = String.Empty;
        Deadline = DateTime.Now.Date;
        Category = new Category();
        Location = new Address();
        ClientId = Guid.Empty;
        JobState = State.Open;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Deadline { get; set; }
    public Category Category { get; set; }
    public Address Location { get; set; }
    public Guid ClientId { get; set; }
    public State JobState { get; set; }
}