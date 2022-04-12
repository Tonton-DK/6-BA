using System.Security.Cryptography;

namespace UserService.Classes;

public class Salt
{
    public static string Create()
    {
        byte[] salt = new byte[128 / 8];
        using (var generator = RandomNumberGenerator.Create())
        {
            generator.GetNonZeroBytes(salt);
        }
        return Convert.ToBase64String(salt);
    }
}