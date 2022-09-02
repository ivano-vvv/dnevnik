using System.ComponentModel.DataAnnotations;
using UsersServer.Domain.Users;

namespace UsersServer.Domain.Auth;

public class AuthProfile
{
    public static AuthProfile fromUser(User user, byte[] PasswordSalt, byte[] PasswordHash)
    {
        return new AuthProfile(user.Id, user.Login, PasswordSalt, PasswordHash);
    }
    
    public AuthProfile(Guid UserId, string Login, byte[] PasswordSalt, byte[] PasswordHash)
    {
        this.UserId = UserId;
        this.Login = Login;
        this.PasswordSalt = PasswordSalt;
        this.PasswordHash = PasswordHash;
    }

    // TODO maybe we should have a separated <id - login> dict, since we use the login as a key
    [Required]
    public Guid UserId { get; set; }

    [Key]
    [Required]
    public string Login { get; set; }

    [Required]
    public byte[] PasswordSalt { get; set; }

    [Required]
    public byte[] PasswordHash { get; set; }
}
