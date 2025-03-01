using LangUp.DTOs;
using LangUp.DTOs.Auth;
using LangUp.Models;

namespace LangUp.Services;

public interface IUserService
{
    Task<bool> CreateUserAsync(User user);
    User? GetUserByUsername(string username);
    Task<IEnumerable<User>> GetAllUsersAsync(GetAllUsersRequest? request);
    bool IsValidUser(AuthCredentials authCredentials);
}