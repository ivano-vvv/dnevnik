using Microsoft.AspNetCore.Mvc;
using UsersServer.Controllers.Dependencies;
using UsersServer.Domain.Auth;
using UsersServer.Domain.Auth.Models;
using UsersServer.Domain.Auth.Models.Exceptions;
using UsersServer.Domain.Auth.Repositories;
using UsersServer.Operations.UsersRegistration; // TODO remove dependency

namespace UsersServer.Controllers;


[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase 
{
    private readonly IUsersRegistrationService _registrationService;
    private readonly AuthProfilesRepository _authRepo;

    public AuthController(
        IUsersRegistrationService registrationService,
        AuthProfilesRepository authRepo)
    {
        _registrationService = registrationService;
        _authRepo = authRepo;
    }
    
    [HttpPost("sign-up")]
    public async Task<ActionResult<AuthProfile>> create(CreateUserView user)
    {
        try
        {
            var result = await _registrationService.register(user);
            return Ok(result);
        }
        catch (UserExistsException e)
        {
            return BadRequest(e.Message);
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
    }
}
