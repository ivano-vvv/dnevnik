using UsersServer.Infrastructure.AppDatabase;
using UsersServer.Domain.Auth.Dependencies;
using UsersServer.Domain.Values.Credentials;
using UsersServer.Domain.Users;

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
        var existingUser = _dbContext.AuthProfiles.FirstOrDefault(profile => profile.Login == login);
        return existingUser == null;
    }

    public async Task<SessionTokens> create(User user, Credentials credentials)
    {
        var encodedPassword = _cryptoService.encode(credentials.Password);
        var profile = AuthProfile.fromUser(user, encodedPassword.Salt, encodedPassword.Hash);

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
