using LangUp.Database;
using LangUp.Models;
using Microsoft.EntityFrameworkCore;

namespace LangUp.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _dbContext;

    public UserService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        if (await _dbContext.Users.AnyAsync(u => u.Username == user.Username))
        {
            return false;
        }

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public User? GetUserByUsername(string username)
    {
        return _dbContext.Users.SingleOrDefault(u => u.Username == username);
    }
}