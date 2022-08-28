using System.ComponentModel.DataAnnotations;

namespace UsersServer.Domain.Users;

public class User {
    public User(string Login, string Name = "")
    {
        this.Id = Guid.NewGuid();

        this.Login = Login;
        this.Name = Name;
    }

    [Key]
    [Required]
    public Guid Id { get; set; }

    [Key]
    [Required]
    public string Login { get; set; }

    public string Name { get; set; }
}
