using UsersServer.Domain.Auth;
using UsersServer.Operations.UsersRegistration;

namespace UsersServer.Controllers.Dependencies;

public interface IUsersRegistrationService
{
    public Task<AuthProfile> register(CreateUserView user);
}