using System.ComponentModel.DataAnnotations;

namespace UsersServer.Domain.Auth;


public class SessionTokens
{
    public SessionTokens(string accessToken)
    {
        this.AccessToken = accessToken;
    }

    [Required]
    public string AccessToken { get; set; }
}
