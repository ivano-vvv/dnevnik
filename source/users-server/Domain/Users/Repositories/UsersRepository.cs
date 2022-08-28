using UsersServer.Infrastructure.AppDatabase;

namespace UsersServer.Domain.Users.Repositories;

public class UsersRepository
{
    public UsersRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private AppDbContext _dbContext;
}
