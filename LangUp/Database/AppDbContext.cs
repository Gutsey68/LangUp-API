namespace LangUp.Database;

using Microsoft.EntityFrameworkCore;
using LangUp.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Translation> Translations { get; set; }
    public DbSet<User> Users { get; set; }
}
