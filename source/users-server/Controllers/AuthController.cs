using Microsoft.AspNetCore.Mvc;
using UsersServer.Domain.Auth;
using UsersServer.Domain.Auth.Repositories;
using UsersServer.Domain.Auth.Utils.Exceptions;
using UsersServer.Domain.Users.Repositories;
using UsersServer.Domain.Users.Utils.Exceptions;
using UsersServer.Domain.Users.Views;
using UsersServer.Domain.Values.Credentials;

namespace UsersServer.Controllers;

// TODO fix warnings
[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase 
{
    private readonly AuthProfilesRepository _authRepo;
    private readonly UsersRepository _usersRepo;
    private readonly SessionTokensRepository _tokensRepo;
    private readonly IHttpContextAccessor _httpAccessor;

    public AuthController(
        AuthProfilesRepository authRepo,
        UsersRepository usersRepo,
        SessionTokensRepository tokensRepo
    )
    {
        _authRepo = authRepo;
        _usersRepo = usersRepo;
        _tokensRepo = tokensRepo;
    }
    
    [HttpPost("sign-up")]
    public async Task<ActionResult<SessionTokens>> create(CreateUserView user)
    {
        try
        {
            var result = await _usersRepo.create(user);
            return Ok(result);
        }
        catch (UserExistsException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            // TODO log error
            return StatusCode(500);
        }
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<SessionTokens>> getAccessToken(Credentials credentials)
    {
        try
        {
            var tokens = await _authRepo.getSessionsTokens(credentials);
            return Ok(tokens);
        }
        catch (InvalidCredentialsException e)
        {
            return Unauthorized(e.Message);
        }
        catch (Exception e)
        {
            // TODO log error
            return StatusCode(500);
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<SessionTokens>> refreshToken(string accessToken, string refreshToken)
    {
        try
        {
            return await _tokensRepo.refresh(accessToken, refreshToken);
        }
        catch (Exception e)
        {
            if (e is ValidRefreshTokenNotFoundException || e is InvalidAccessTokenException)
            {
                // TODO log smth
                return Unauthorized(e.Message);
            }

            // TODO log smth
            return StatusCode(500);
        }
    }
}
