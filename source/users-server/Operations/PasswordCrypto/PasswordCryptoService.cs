using System.Security.Cryptography;
using System.Text;
using UsersServer.Domain.Auth.Dependencies;
using UsersServer.Domain.Auth.Models;

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
}
