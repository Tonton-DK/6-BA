namespace ClassLibrary.Classes;

public class User
{
    public User(Guid id, string email, string firstName, string lastName, string phoneNumber, string profilePicture, bool isServiceProvider)
    {
        Id = id;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        ProfilePicture = profilePicture;
        IsServiceProvider = isServiceProvider;
    }

    public Guid Id { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string ProfilePicture { get; set; }
    public bool IsServiceProvider { get; set; }
}

public class UserCreator : User
{
    public UserCreator(Guid id, string email, string firstName, string lastName, string phoneNumber, string profilePicture, bool isServiceProvider, string password) 
        : base(id, email, firstName, lastName, phoneNumber, profilePicture, isServiceProvider)
    {
        Password = password;
    }
    
    public UserCreator() 
        : base(Guid.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false)
    {
        Password = string.Empty;
    }
    
    public string Password { get; set; }
}

public class UserValidator : User
{
    public UserValidator(Guid id, string email, string firstName, string lastName, string phoneNumber, string profilePicture, bool isServiceProvider, string salt, string hash) 
        : base(id, email, firstName, lastName, phoneNumber, profilePicture, isServiceProvider)
    {
        PasswordSalt = salt;
        PasswordHash = hash;
    }

    public string PasswordSalt { get; set; }
    public string PasswordHash { get; set; }
}