using System.ComponentModel.DataAnnotations;

namespace UsersServer.Domain.Users;

public class User {
    public User(string Login, string Name = "")
    {
        // TODO consider storing Id as a string
        this.Id = Guid.NewGuid();

        this.Login = Login;
        this.Name = Name;
    }

    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    public string Login { get; set; }

    public string Name { get; set; }
}
