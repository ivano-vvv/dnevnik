namespace UsersServer.Domain.Values.Credentials;

public class InvalidCredentialsException : Exception {
    public InvalidCredentialsException (string login)
        : base($"Credentials for user '{login}' are not correct or the user doesn't exist.") {}
}

