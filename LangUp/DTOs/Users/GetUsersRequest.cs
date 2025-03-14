namespace LangUp.DTOs;

public class GetUsersRequest
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public ICollection<Models.Translation> Translations { get; set; } = new List<Models.Translation>();
}