using LangUp.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LangUp.Services;

public class AuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;
    
    public AuthService(IConfiguration configuration, IUserService userService)
    {
        _configuration = configuration;
        _userService = userService;
    }
    
    // public string GenerateToken([FromBody] AuthCredentials credentials)
    // {
    //     if (!_userService.IsValidUser(credentials)) return Unauthorized();
    //     
    //     var claims = new[]
    //     {
    //         new Claim(JwtRegisteredClaimNames.Sub, credentials.Username!),
    //         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    //     };
    //
    //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
    //     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    //
    //     var token = new JwtSecurityToken(
    //         issuer: _configuration["Jwt:Issuer"],
    //         audience: _configuration["Jwt:Audience"],
    //         claims: claims,
    //         expires: DateTime.Now.AddMinutes(30),
    //         signingCredentials: creds);
    //
    //     return new JwtSecurityTokenHandler().WriteToken(token);
    //
    // }
}