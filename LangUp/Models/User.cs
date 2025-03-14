namespace LangUp.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Email { get; set; }
    public required string RefreshToken { get; set; } = null!;
    
    public ICollection<Translation> Translations { get; set; } = new List<Translation>();
}