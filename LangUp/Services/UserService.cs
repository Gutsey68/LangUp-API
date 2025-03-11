using LangUp.Database;
using LangUp.DTOs;
using LangUp.DTOs.Auth;
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
    
    public async Task<IEnumerable<User>> GetAllUsersAsync(GetAllUsersRequest? request)
    {
        int page = request?.Page ?? 1;
        int numberOfRecords = request?.RecordsPerPage ?? 100;
        
        IQueryable<User> query = _dbContext.Users
            .Skip((page - 1) * numberOfRecords)
            .Take(numberOfRecords);
        
        if (request != null)
        {
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                query = query.Where(e => e.Username.Contains(request.Username));
            }
            
            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                query = query.Where(e => e.Email.Contains(request.Email));
            }
        }
        
        return await query.ToListAsync();
    }

    public User? GetUserByUsername(string username)
    {
        return _dbContext.Users.SingleOrDefault(u => u.Username == username);
    }
    
    public async Task<User?> ValidateUserAsync(AuthCredentials credentials)
    {
        var user = await _dbContext.Users
            .SingleOrDefaultAsync(u => u.Username == credentials.Username);
    
        if (user != null && BCrypt.Net.BCrypt.Verify(credentials.Password!, user.Password))
        {
            return user;
        }
    
        return null;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        _dbContext.Users.Update(user);
        var savedEntities = await _dbContext.SaveChangesAsync();
        return savedEntities > 0;
    }
    
    public bool IsValidUser(AuthCredentials credentials)
    {
        var user = GetUserByUsername(credentials.Username!);
        return user != null && BCrypt.Net.BCrypt.Verify(credentials.Password!, user.Password);
    }
}