namespace ClassLibrary;

public class User
{
    public User(int id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public int Id { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
}