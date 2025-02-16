using LangUp.DTOs.Auth;
using LangUp.Models;
using LangUp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LangUp.Controllers;

public class AuthController : BaseController
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public AuthController(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="request">The user registration request.</param>
    /// <returns>The newly created user.</returns>
    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterRequest request)
    {
        var user = new User
        {
            Username = request.Username!,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password!),
            Email = request.Email!,
        };

        var result = await _userService.CreateUserAsync(user);
        if (!result)
        {
            return BadRequest("User registration failed.");
        }

        var response = new UserResponse
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email
        };

        return Ok(response);
    }
    
    [HttpPost("token")]
    public IActionResult GenerateToken([FromBody] AuthCredentials credentials)
    {
        if (IsValidUser(credentials))
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, credentials.Username!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        return Unauthorized();
    }

    private bool IsValidUser(AuthCredentials credentials)
    {
        var user = _userService.GetUserByUsername(credentials.Username!);
        return user != null && BCrypt.Net.BCrypt.Verify(credentials.Password!, user.Password);
    }
}