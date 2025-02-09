namespace LangUp.Models;

public class Translation 
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string OriginalText { get; set; }
    public required string TranslatedText { get; set; }
    public required string SourceLanguage { get; set; }
    public required string TargetLanguage { get; set; }
}
