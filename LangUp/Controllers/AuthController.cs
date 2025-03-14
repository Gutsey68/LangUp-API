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
            RefreshToken = string.Empty
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
    
    /// <summary>
    /// Log a user.
    /// </summary>
    /// <returns/> The access token and the fresh token
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] AuthCredentials credentials)
    {
        var user = await _userService.ValidateUserAsync(credentials);
        if (user == null)
        {
            return Unauthorized();
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, credentials.Username!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("UserId", user.Id.ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        

        var refreshToken = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    
        user.RefreshToken = refreshToken;
        await _userService.UpdateUserAsync(user);

        return Ok(new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }
}