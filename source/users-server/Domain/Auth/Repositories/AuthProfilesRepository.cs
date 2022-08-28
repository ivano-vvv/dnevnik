using UsersServer.Infrastructure.AppDatabase;
using UsersServer.Domain.Auth.Models;
using UsersServer.Domain.Auth.Dependencies;

namespace UsersServer.Domain.Auth.Repositories;

public class AuthProfilesRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordCryptoService _cryptoService;

    public AuthProfilesRepository(AppDbContext dbContext, IPasswordCryptoService cryptoService)
    {
        _dbContext = dbContext;
        _cryptoService = cryptoService;
    }

    public bool isLoginUnique(string login)
    {
        return _dbContext.Find<AuthProfile>(login) == null;
    }

    public async Task<AuthProfile> create(Credentials credentials)
    {
        var encodedPassword = _cryptoService.encode(credentials.Password);
        var profile = new AuthProfile(credentials.Login, encodedPassword.Salt, encodedPassword.Hash);

        await _dbContext.AuthProfiles.AddAsync(profile);
        await saveChanges();

        return profile;
    }

    private async Task<bool> saveChanges()
    {
        return (await _dbContext.SaveChangesAsync() >= 0);
    } 
}
