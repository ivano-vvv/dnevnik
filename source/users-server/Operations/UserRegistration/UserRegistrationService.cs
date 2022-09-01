using UsersServer.Controllers.Dependencies;
using UsersServer.Domain.Auth;
using UsersServer.Domain.Auth.Repositories;

namespace UsersServer.Operations.UsersRegistration;

public class UserRegistrationService : IUsersRegistrationService
{
    private readonly AuthProfilesRepository _authRepo;

    public UserRegistrationService(AuthProfilesRepository authRepo)
    {
        _authRepo = authRepo;
    }

    public async Task<SessionTokens> register(CreateUserView user)
    {
        if (_authRepo.isLoginUnique(user.Credentials.Login) == false)
        {
            throw new UserExistsException(user.Credentials.Login);
        }

        var Id = Guid.NewGuid();

        return await _authRepo.create(user.Credentials);
    }
}
