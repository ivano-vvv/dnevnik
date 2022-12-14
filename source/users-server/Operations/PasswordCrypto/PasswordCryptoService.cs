using System.Security.Cryptography;
using System.Text;
using UsersServer.Domain.Auth.Dependencies;

namespace UsersServer.Operations.PasswordCrypto;

public class PasswordCryptoService : IPasswordCryptoService
{
    public PasswordEncodingResult encode(string password)
    {
        using(var hmac = new HMACSHA512())
        {
            var salt = hmac.Key;
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return new PasswordEncodingResult(salt, hash);
        }
    }

    public bool verify(string password, byte[] salt, byte[] hash)
    {
        using(var hmac = new HMACSHA512(salt))
        {
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(hash);
        }
    }
}
