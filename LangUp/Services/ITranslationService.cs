using LangUp.DTOs.Translation;
using LangUp.Models;

namespace LangUp.Services
{
    public interface ITranslationService
    {
        Task<IEnumerable<Translation>> GetAllTranslationsAsync(GetAllTranslationsRequest? request);
        Task<Translation?> GetTranslationByIdAsync(int id);
        Task<Translation> CreateTranslationAsync(CreateTranslationRequest translationRequest);
        Task<bool> DeleteTranslationAsync(int id);
        
    }
}