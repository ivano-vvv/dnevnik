using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UsersServer.Domain.Auth.Utils.Exceptions;
using UsersServer.Helpers;
using UsersServer.Infrastructure.AppDatabase;

namespace UsersServer.Domain.Auth.Repositories;

public class SessionTokensRepository
{
    private readonly string _accessTokenKey;
    private readonly AppDbContext _dbContext;
    private readonly JwtSecurityTokenHandler _jwtHandler;

    public SessionTokensRepository(IConfiguration config, AppDbContext dbContext)
    {
        _accessTokenKey = config.GetValue<string>("ACCESS_TOKEN_KEY");

        if (_accessTokenKey == null)
        {
            throw new Exception("ACCESS_TOKEN_KEY is not provided");
        }

        _dbContext = dbContext;
        _jwtHandler = new JwtSecurityTokenHandler();
    }

    public SessionTokens get(string UserId)
    {
        var claims = new List<Claim>
        {
            new Claim("UserId", UserId.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_accessTokenKey));
        var signingCreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var jwt = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddSeconds(60), // TEMP
            signingCredentials: signingCreds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        var refreshToken = RefreshToken.gen(UserId);

        saveRefreshToken(refreshToken);

        return new SessionTokens(accessToken, refreshToken.Value);
    }

    public async Task<SessionTokens> refresh(string AccessToken, string RefreshTokenValue)
    {
        var jwt = decryptAccessToken(AccessToken);
        var UserId = ClaimsHelper.getValueByKey(jwt.Claims, "UserId");

        if (UserId == null)
        {
            throw new InvalidAccessTokenException();
        }
        
        var savedToken = await _dbContext.RefreshTokens.FindAsync(Guid.Parse(UserId));
        
        if (savedToken == null || savedToken.Expires <= DateTime.Now)
        {
            throw new ValidRefreshTokenNotFoundException(UserId);
        }

        return this.get(UserId);
    }

    private JwtSecurityToken decryptAccessToken(string accessTokenValue)
    {
        if (validateAccessToken(accessTokenValue) == false)
        {
            throw new InvalidAccessTokenException();
        }

        try
        {
            return _jwtHandler.ReadJwtToken(accessTokenValue);
        }
        catch(Exception)
        {
            // TODO log smth
            throw new InvalidAccessTokenException();
        }
    }

    private bool validateAccessToken(string accessTokenValue)
    {
        SecurityToken validatedToken;

        try
        {
            _jwtHandler.ValidateToken(
                accessTokenValue, 
                new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_accessTokenKey)
                        )
                    },
                out validatedToken
            );

            return true;
        }
        catch(Exception)
        {
            // TODO log smth
            return false;
        }
    }

    private async void saveRefreshToken(RefreshToken token)
    {
        var savedToken = _dbContext.RefreshTokens.FirstOrDefault(
            saved => saved.UserId == token.UserId
        );
        
        if (savedToken != null)
        {
            savedToken = token;
        }
        else
        {
            await _dbContext.RefreshTokens.AddAsync(token);
        }

        await saveDbChanges();
    }

    private async Task<bool> saveDbChanges()
    {
        return (await _dbContext.SaveChangesAsync()) >= 0;
    }
}
