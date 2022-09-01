using UsersServer.Domain.Auth;

namespace UsersServer.Infrastructure.AppDatabase;

public static class PrepAppDB
{
    private static readonly List<AuthProfile> DEFAULT_AUTH_PROFILES = new List<AuthProfile>();

    public static void PrepPopulation(WebApplication app)
    {
        using var servicesScope = app.Services.CreateScope();
        PopulateDb(servicesScope.ServiceProvider.GetService<AppDbContext>());
    }

    private static void PopulateDb(AppDbContext? context)
    {
        if (context == null)
        {
            throw new Exception("db context is not set up");
        }

        if (context.AuthProfiles.Any() == false) {
            foreach (var profile in DEFAULT_AUTH_PROFILES)
            {
                context.AuthProfiles.Add(profile);
            }
        }

        context.SaveChanges();
    }
}