using LangUp.Models;

namespace LangUp.Services;

public class TranslationService
{
    static List<Translation> Translation {get;}

    static TranslationService()
    {
        Translation = new List<Translation>
        {
            new Translation
            {
                Id = 1,
                UserId = 1,
                OriginalText = "Hello",
                TranslatedText = "Hola",
                SourceLanguage = "English",
                TargetLanguage = "Spanish"
            },
            new Translation
            {
                Id = 2,
                UserId = 1,
                OriginalText = "Goodbye",
                TranslatedText = "Adiós",
                SourceLanguage = "English",
                TargetLanguage = "Spanish"
            }
        };
    }


    public static List<Translation> GetAll() => Translation;
}