using UsersServer.Domain.Auth.Models;

namespace UsersServer.Operations.UsersRegistration;

public class CreateUserView
{
    public CreateUserView(string name, Credentials credentials)
    {
        this.Name = name;
        this.Credentials = credentials;
    }

    public string Name { get; set; }

    public Credentials Credentials { get; set; }
}
