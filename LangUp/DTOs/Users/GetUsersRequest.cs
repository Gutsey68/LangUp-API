namespace LangUp.DTOs;

public class GetUsersRequest
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
}