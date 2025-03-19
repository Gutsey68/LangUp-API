
using LangUp.Database;
using Microsoft.EntityFrameworkCore;

namespace LangUp.Extensions;

public static class ApplicationExtensions
{
    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}