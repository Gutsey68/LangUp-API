using LangUp.Database;
using LangUp.DTOs.Translation;
using LangUp.Models;
using Microsoft.EntityFrameworkCore;

namespace LangUp.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly AppDbContext _dbContext;

        public TranslationService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Translation>> GetAllTranslationsAsync(GetAllTranslationsRequest? request)
        {
            int page = request?.Page ?? 1;
            int numberOfRecords = request?.RecordsPerPage ?? 100;

            IQueryable<Translation> query = _dbContext.Translations
                .Skip((page - 1) * numberOfRecords)
                .Take(numberOfRecords);

            if (request != null)
            {
                if (!string.IsNullOrWhiteSpace(request.OriginalText))
                {
                    query = query.Where(e => e.OriginalText.Contains(request.OriginalText));
                }

                if (!string.IsNullOrWhiteSpace(request.TranslatedText))
                {
                    query = query.Where(e => e.TranslatedText.Contains(request.TranslatedText));
                }

                if (!string.IsNullOrWhiteSpace(request.SourceLanguage))
                {
                    query = query.Where(e => e.SourceLanguage.Contains(request.SourceLanguage));
                }

                if (!string.IsNullOrWhiteSpace(request.TargetLanguage))
                {
                    query = query.Where(e => e.TargetLanguage.Contains(request.TargetLanguage));
                }
            }

            return await query.ToListAsync();
        }

        public async Task<Translation?> GetTranslationByIdAsync(int id)
        {
            return await _dbContext.Translations.SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Translation> CreateTranslationAsync(CreateTranslationRequest translationRequest)
        {
            var newTranslation = new Translation
            {
                OriginalText = translationRequest.OriginalText!,
                TranslatedText = translationRequest.TranslatedText!,
                SourceLanguage = translationRequest.SourceLanguage!,
                TargetLanguage = translationRequest.TargetLanguage!
            };

            _dbContext.Translations.Add(newTranslation);
            await _dbContext.SaveChangesAsync();

            return newTranslation;
        }

        public async Task<bool> DeleteTranslationAsync(int id)
        {
            var translation = await _dbContext.Translations.FindAsync(id);

            if (translation == null)
            {
                return false;
            }

            _dbContext.Translations.Remove(translation);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        
    }
}