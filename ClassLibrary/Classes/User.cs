namespace ClassLibrary.Classes;

public class User
{
    public User(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid Id { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
}