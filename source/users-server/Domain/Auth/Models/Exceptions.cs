namespace UsersServer.Domain.Auth.Models.Exceptions;

public class InvalidCredentialsException : Exception {
    public InvalidCredentialsException (string login)
        : base($"Credentials for user '{login}' are not correct or the user doesn't exist.") {}
}

