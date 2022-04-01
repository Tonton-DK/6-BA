namespace ClassLibrary.Classes;

public class PasswordData
{
    public PasswordData(Guid userId, string oldPassword, string newPassword)
    {
        UserId = userId;
        OldPassword = oldPassword;
        NewPassword = newPassword;
    }
    
    public Guid UserId { get; set; }
    
    public string OldPassword { get; set; }
    
    public string NewPassword { get; set; }
}