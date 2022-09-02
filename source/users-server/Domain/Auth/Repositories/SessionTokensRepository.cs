using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace UsersServer.Domain.Auth.Repositories;

public class SessionTokensRepository
{
    private readonly string _accessTokenKey;

    public SessionTokensRepository(IConfiguration config)
    {
        _accessTokenKey = config.GetValue<string>("ACCESS_TOKEN_KEY");

        if (_accessTokenKey == null)
        {
            throw new Exception("ACCESS_TOKEN_KEY is not provided");
        }
    }

    public SessionTokens get(AuthProfile profile)
    {
        var claims = new List<Claim>
        {
            new Claim("UserId", profile.UserId.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_accessTokenKey));
        var signingCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var jwt = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: signingCreds
        );

        var jwtStr = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new SessionTokens(jwtStr);
    }
}
