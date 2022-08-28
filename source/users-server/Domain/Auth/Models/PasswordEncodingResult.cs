namespace UsersServer.Domain.Auth.Models;

public class PasswordEncodingResult
{
    public PasswordEncodingResult(byte[] salt, byte[] hash)
    {
        Salt = salt;
        Hash = hash;        
    }
    
    public byte[] Salt { get; set; }

    public byte[] Hash { get; set; }
}
