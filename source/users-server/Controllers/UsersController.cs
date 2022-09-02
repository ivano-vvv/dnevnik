using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UsersServer.Domain.Users.Repositories;
using UsersServer.Domain.Users.Utils.Exceptions;
using UsersServer.Domain.Users.Views;
using UsersServer.Helpers;

namespace UsersServer.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UsersRepository _usersRepo;
    private readonly IHttpContextAccessor _httpAccessor;

    public UsersController(
        UsersRepository usersRepo,
        IHttpContextAccessor httpAccessor    
    )
    {
        _usersRepo = usersRepo;
        _httpAccessor = httpAccessor;
    }
    
    [HttpGet("me"), Authorize]
    public async Task<ActionResult<ReadUserView>> getCurrentUser()
    {   
        var context = _httpAccessor.HttpContext;
        var userId = "";

        if (context == null)
        {
            return BadRequest("Broken request identity.");
        }

        try
        {
            userId = ClaimsHelper.getValueByKey(
                context.User.Claims, "UserId"
            );
        }
        catch(ClaimNotFoundException e)
        {
            return BadRequest("Broken request identity.");
        }

        try
        {
            var user = await _usersRepo.getUserById(userId);
            return new ReadUserView(user);
        }
        catch(UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch(Exception e)
        {
            // TODO add logging
            return StatusCode(500);
        }
    }
}
