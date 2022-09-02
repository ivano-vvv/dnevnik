namespace UsersServer.Domain.Auth.Utils.Exceptions;

public class ValidRefreshTokenNotFoundException : Exception
{
    // TODO use for every logs either id or login
    public ValidRefreshTokenNotFoundException(string UserId)
        : base($"Refresh Token for the user \"{UserId}\" has not been found.") {}
}

public class InvalidAccessTokenException : Exception
{
    // TODO use for every logs either id or login
    public InvalidAccessTokenException()
        : base($"Invalid access token.") {}
}
