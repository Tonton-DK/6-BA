namespace ClassLibrary.Classes;

public class User
{
    public User(Guid id, string email, string password, string firstName, string lastName, string phoneNumber, bool isServiceProvider)
    {
        Id = id;
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        IsServiceProvider = isServiceProvider;
    }

    public Guid Id { get; set; }

    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public bool IsServiceProvider { get; set; }
    
    //public Address Address { get; set; }
    //public CV CV { get; set; }
}