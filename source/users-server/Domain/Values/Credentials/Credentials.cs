namespace UsersServer.Domain.Values.Credentials;

public class Credentials
{
    public Credentials(string login, string password)
    {
        this.Login = login;
        this.Password = password;
    }

    public string Login { get; set; }

    public string Password { get; set; }
}