using System.Security.Claims;

namespace UsersServer.Helpers;

public static class ClaimsHelper
{
    public static string getValueByKey(IEnumerable<Claim> claims, string key)
    {
        var claim = claims.FirstOrDefault(c => c.Type == key);

        if (claim == null)
        {
            throw new ClaimNotFoundException(key);
        }

        return claim.Value;
    }
}

public class ClaimNotFoundException : Exception
{
    public ClaimNotFoundException(string key)
        : base($"Claim with the \"{key}\" key has not been found.") {}
}