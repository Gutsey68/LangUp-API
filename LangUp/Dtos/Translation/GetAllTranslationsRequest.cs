using LangUp.Models;

namespace LangUp.DTOs.Translation;

public class GetAllTranslationsRequest
{
    public int? Page { get; set; }
    public int? RecordsPerPage { get; set; }
    
    public string? OriginalText { get; set; }
    public string? TranslatedText { get; set; }
    public string? SourceLanguage { get; set; }
    public string? TargetLanguage { get; set; }
    public int UserId { get; set; }
}