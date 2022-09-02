using UsersServer.Domain.Auth;
using UsersServer.Domain.Auth.Repositories;
using UsersServer.Domain.Users.Utils.Exceptions;
using UsersServer.Domain.Users.Views;
using UsersServer.Infrastructure.AppDatabase;

namespace UsersServer.Domain.Users.Repositories;

public class UsersRepository
{
    private readonly AppDbContext _dbContext;
    private readonly AuthProfilesRepository _authRepo;

    public UsersRepository(
        AppDbContext dbContext,
        AuthProfilesRepository authRepo,
        IHttpContextAccessor httpAccessor
    )
    {
        _dbContext = dbContext;
        _authRepo = authRepo;
    }

    public async Task<SessionTokens> create(CreateUserView userInput)
    {
        if (_authRepo.isLoginUnique(userInput.Credentials.Login) == false)
        {
            throw new UserExistsException(userInput.Credentials.Login);
        }

        var user = new User(userInput.Credentials.Login, userInput.Name);
        await _dbContext.Users.AddAsync(user); 
        await saveChanges();

        return await _authRepo.create(user, userInput.Credentials);
    }
    
    public async Task<User> getUserById(string Id)
    {
       
        var user = await _dbContext.Users.FindAsync(Guid.Parse(Id));

        if (user == null)
        {
            throw new UserNotFoundException(Id);
        }

        return user;
    }

    private async Task<bool> saveChanges()
    {
        return (await _dbContext.SaveChangesAsync() >= 0);
    }
}
