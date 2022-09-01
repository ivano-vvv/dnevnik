using UsersServer.Domain.Auth;
using UsersServer.Operations.UsersRegistration;

namespace UsersServer.Controllers.Dependencies;

public interface IUsersRegistrationService
{
    public Task<SessionTokens> register(CreateUserView user);
}