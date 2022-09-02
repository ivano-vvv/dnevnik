namespace UsersServer.Domain.Users.Utils.Exceptions;

public class UserNotFoundException : Exception
{
    public UserNotFoundException(string loginOrId)
        : base($"User \"{loginOrId}\" has not been found") {}
}

public class UserExistsException : Exception
{
    public UserExistsException(string login) : 
        base($"Cannot create user : login '{login}' is already used") {}
}
