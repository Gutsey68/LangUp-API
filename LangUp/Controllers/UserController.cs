using LangUp.DTOs;
using LangUp.Models;
using LangUp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LangUp.Controllers;

[Authorize]
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
    
    /// <summary>
    /// Retrieves one user.
    /// </summary>
    /// <returns>A list of users.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetUsersRequest), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUser([FromRoute] int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        
        if (user == null)
        {
            return NotFound();
        }
        
        return Ok(UserToGetUserResponse(user));
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