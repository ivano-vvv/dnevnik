using Microsoft.AspNetCore.Mvc;
using UsersServer.Controllers.Dependencies;
using UsersServer.Domain.Auth;
using UsersServer.Operations.UsersRegistration; // TODO remove dependency

namespace UsersServer.Controllers;


[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase 
{
    private readonly IUsersRegistrationService _registrationService;

    public AuthController(IUsersRegistrationService registrationService)
    {
        _registrationService = registrationService;
    }
    
    [HttpPost]
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
}
