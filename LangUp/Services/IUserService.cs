using LangUp.DTOs;
using LangUp.DTOs.Auth;
using LangUp.Models;

namespace LangUp.Services;

public interface IUserService
{
    Task<bool> CreateUserAsync(User user);
    Task<IEnumerable<User>> GetAllUsersAsync(GetAllUsersRequest? request);
    Task<User?> ValidateUserAsync(AuthCredentials credentials);
    Task<bool> UpdateUserAsync(User user);
    bool IsValidUser(AuthCredentials credentials);
}