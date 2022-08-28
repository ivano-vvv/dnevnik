using System.ComponentModel.DataAnnotations;

namespace UsersServer.Domain.Auth;

public class AuthProfile
{
    public AuthProfile(string Login, byte[] PasswordSalt, byte[] PasswordHash)
    {
        this.Login = Login;
        this.PasswordSalt = PasswordSalt;
        this.PasswordHash = PasswordHash;
    }

    [Key]
    [Required]
    public string Login { get; set; }

    [Required]
    public byte[] PasswordSalt { get; set; }

    [Required]
    public byte[] PasswordHash { get; set; }
}
