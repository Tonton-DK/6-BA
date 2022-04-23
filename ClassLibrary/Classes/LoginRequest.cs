namespace ClassLibrary.Classes;

public class LoginRequest
{
    public LoginRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }
    
    public LoginRequest()
    {
        Email = string.Empty;
        Password = string.Empty;
    }

    public string Email { get; set; }
    public string Password { get; set; }
}