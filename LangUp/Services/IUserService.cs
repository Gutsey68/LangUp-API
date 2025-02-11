using LangUp.Models;

namespace LangUp.Services;

public interface IUserService
{
    Task<bool> CreateUserAsync(User user);
    User? GetUserByUsername(string username);
}