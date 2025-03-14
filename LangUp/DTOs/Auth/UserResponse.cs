namespace LangUp.DTOs.Auth;

public class UserResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public ICollection<Models.Translation> Translations { get; set; } = new List<Models.Translation>();
}