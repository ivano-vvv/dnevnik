namespace UsersServer.Operations.UsersRegistration;

public class UserExistsException : Exception
{
    public UserExistsException(string login) : 
        base($"Cannot create user : login '{login}' is already used") {}
}
