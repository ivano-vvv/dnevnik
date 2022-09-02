using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace UsersServer.Domain.Auth;


public class SessionTokens
{
    public SessionTokens(string AccessToken, string RefreshToken)
    {
        this.AccessToken = AccessToken;
        this.RefreshToken = RefreshToken;
    }

    [Required]
    public string AccessToken { get; set; }

    public string RefreshToken { get; set; }
}

public class RefreshToken
{
    public static RefreshToken gen(string UserId)
    {
        return new RefreshToken(
            Guid.Parse(UserId),
            Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            DateTime.Now.AddDays(7) // TODO store value in the configuration
        );
    }
    
    public RefreshToken(Guid UserId, string Value, DateTime Expires)
    {
        this.UserId = UserId;
        this.Value = Value;
        this.Expires = Expires;
    }

    [Key]
    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string Value { get; set; }

    [Required]
    public DateTime Expires { get; set; }
}