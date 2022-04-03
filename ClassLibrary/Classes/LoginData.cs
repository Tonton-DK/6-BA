namespace ClassLibrary.Classes;

public class LoginData
{
    public LoginData(string email, string password)
    {
        Email = email;
        Password = password;
    }

    public string Email { get; set; }

    public string Password { get; set; }
}