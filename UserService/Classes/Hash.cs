using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace UserService.Classes;

public class Hash
{
    public static string Create(string password, string salt)
    {
        string hash = Convert.ToBase64String(
            KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.UTF8.GetBytes(salt),
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        return hash;
    }

    public static bool Validate(string password, string salt, string hash)
        => Create(password, salt) == hash;
}