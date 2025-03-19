using LangUp.Models;

namespace LangUp.DTOs.Translation;

public class GetTranslationRequest
{
    public int Id { get; set; }
    public string? OriginalText { get; set; }
    public string? TranslatedText { get; set; }
    public string? SourceLanguage { get; set; }
    public string? TargetLanguage { get; set; }
    
    public int UserId { get; set; }
}
