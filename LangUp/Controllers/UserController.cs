using LangUp.DTOs;
using LangUp.Models;
using LangUp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LangUp.Controllers;

public class UsersController : BaseController
{
    private readonly IUserService _userService;
    
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    
    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <param name="request">The request parameters for filtering users.</param>
    /// <returns>A list of users.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetUsersRequest>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersRequest? request)
    {
        var users = await _userService.GetAllUsersAsync(request);
            
        return Ok(users.Select(UserToGetUserResponse));
    }
    
    private static GetUsersRequest UserToGetUserResponse(User user)
    {
        return new GetUsersRequest
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
        };
    }
}