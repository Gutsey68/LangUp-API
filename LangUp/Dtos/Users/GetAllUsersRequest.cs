namespace LangUp.DTOs;

public class GetAllUsersRequest
{
    public int? Page { get; set; }
    public int? RecordsPerPage { get; set; }
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    
}