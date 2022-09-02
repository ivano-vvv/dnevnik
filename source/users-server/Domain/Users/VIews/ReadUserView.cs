using System.ComponentModel.DataAnnotations;
using UsersServer.Domain.Users;

namespace UsersServer.Domain.Users.Views;

public class ReadUserView
{
    public ReadUserView(User user)
    {
        this.Id = user.Id;
        this.Name = user.Name;
    }

    [Key]
    [Required]
    public Guid Id { get; set; }

    public string? Name { get; set; }
}