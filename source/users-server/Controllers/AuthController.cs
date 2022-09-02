using Microsoft.AspNetCore.Mvc;
using UsersServer.Domain.Auth;
using UsersServer.Domain.Auth.Repositories;
using UsersServer.Domain.Users.Repositories;
using UsersServer.Domain.Users.Utils.Exceptions;
using UsersServer.Domain.Users.Views;
using UsersServer.Domain.Values.Credentials;

namespace UsersServer.Controllers;


[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase 
{
    private readonly AuthProfilesRepository _authRepo;
    private readonly UsersRepository _usersRepo;

    public AuthController(AuthProfilesRepository authRepo, UsersRepository usersRepo)
    {
        _authRepo = authRepo;
        _usersRepo = usersRepo;
    }
    
    [HttpPost("sign-up")]
    public async Task<ActionResult<AuthProfile>> create(CreateUserView user)
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
    public async Task<ActionResult<string>> getAccessToken(Credentials credentials)
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
}
