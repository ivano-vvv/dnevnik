using UsersServer.Infrastructure.AppDatabase;
using UsersServer.Domain.Auth.Models;
using UsersServer.Domain.Auth.Dependencies;
using UsersServer.Domain.Auth.Models.Exceptions;

namespace UsersServer.Domain.Auth.Repositories;

public class AuthProfilesRepository
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordCryptoService _cryptoService;
    private readonly SessionTokensRepository _tokensRepo;

    public AuthProfilesRepository(
        AppDbContext dbContext,
        IPasswordCryptoService cryptoService,
        SessionTokensRepository tokensRepo)
    {
        _dbContext = dbContext;
        _cryptoService = cryptoService;
        _tokensRepo = tokensRepo;
    }

    public bool isLoginUnique(string login)
    {
        return _dbContext.Find<AuthProfile>(login) == null;
    }

    public async Task<SessionTokens> create(Credentials credentials)
    {
        var encodedPassword = _cryptoService.encode(credentials.Password);
        var profile = new AuthProfile(credentials.Login, encodedPassword.Salt, encodedPassword.Hash);

        await _dbContext.AuthProfiles.AddAsync(profile);
        await saveChanges();

        return _tokensRepo.get(profile);
    }

    public async Task<SessionTokens> getSessionsTokens(Credentials credentials)
    {
        var profile = await _dbContext.AuthProfiles.FindAsync(credentials.Login);

        if (profile == null)
        {
            throw new InvalidCredentialsException(credentials.Login);
        }

        if (_cryptoService.verify(credentials.Password, profile.PasswordSalt, profile.PasswordHash) == false)
        {
            throw new InvalidCredentialsException(credentials.Login);
        }

        return _tokensRepo.get(profile);
    }

    private async Task<bool> saveChanges()
    {
        return (await _dbContext.SaveChangesAsync() >= 0);
    }
}
