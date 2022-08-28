using UsersServer.Domain.Auth.Models;

namespace UsersServer.Domain.Auth.Dependencies;

public interface IPasswordCryptoService
{
    public PasswordEncodingResult encode(string password);
}
