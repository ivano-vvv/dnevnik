using Microsoft.EntityFrameworkCore;
using UsersServer.Domain.Auth;
using UsersServer.Domain.Users;

namespace UsersServer.Infrastructure.AppDatabase;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions opt) : base(opt) {}

    public DbSet<AuthProfile> AuthProfiles { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasKey(u => new { u.Id, u.Login });
        base.OnModelCreating(builder);
    }
}
