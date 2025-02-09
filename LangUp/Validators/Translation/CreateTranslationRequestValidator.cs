using FluentValidation;
using LangUp.DTOs.Translation;

namespace LangUp.Validators.Translation;

public class CreateTranslationRequestValidator : AbstractValidator<CreateTranslationRequest>
{
    public CreateTranslationRequestValidator()
    {
        RuleFor(x => x.OriginalText).NotEmpty();
        RuleFor(x => x.TranslatedText).NotEmpty();
        RuleFor(x => x.SourceLanguage).NotEmpty();
        RuleFor(x => x.TargetLanguage).NotEmpty();
    }
}