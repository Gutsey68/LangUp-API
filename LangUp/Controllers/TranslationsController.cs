using LangUp.DTOs.Translation;
using LangUp.Models;
using LangUp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LangUp.Controllers
{
    public class TranslationsController : BaseController
    {
        private readonly ITranslationService _translationService;
        private readonly ILogger<TranslationsController> _logger;

        public TranslationsController(ITranslationService translationService, ILogger<TranslationsController> logger)
        {
            _translationService = translationService;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all translations.
        /// </summary>
        /// <param name="request">The request parameters for filtering translations.</param>
        /// <returns>A list of translations.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetTranslationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllTranslations([FromQuery] GetAllTranslationsRequest? request)
        {
            var translations = await _translationService.GetAllTranslationsAsync(request);
            
            return Ok(translations.Select(TranslationToGetTranslationResponse));
        }

        /// <summary>
        /// Retrieves a translation by its ID.
        /// </summary>
        /// <param name="id">The ID of the translation.</param>
        /// <returns>The translation with the specified ID.</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(GetTranslationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTranslationById(int id)
        {
            var translation = await _translationService.GetTranslationByIdAsync(id);
            
            if (translation == null)
            {
                return NotFound();
            }

            var translationResponse = TranslationToGetTranslationResponse(translation);
            
            return Ok(translationResponse);
        }

        /// <summary>
        /// Creates a new translation.
        /// </summary>
        /// <param name="translationRequest">The request containing the translation details.</param>
        /// <returns>The created translation.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetTranslationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTranslation([FromBody] CreateTranslationRequest translationRequest)
        {
            var newTranslation = await _translationService.CreateTranslationAsync(translationRequest);
            
            return CreatedAtAction(nameof(GetTranslationById), new { id = newTranslation.Id }, newTranslation);
        }

        /// <summary>
        /// Deletes a translation by its ID.
        /// </summary>
        /// <param name="id">The ID of the translation to delete.</param>
        /// <returns>A message indicating the result of the deletion.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteTranslation(int id)
        {
            var success = await _translationService.DeleteTranslationAsync(id);
            
            if (!success)
            {
                return NotFound();
            }

            return Ok(new { message = "Translation deleted successfully." });
        }

        private static GetTranslationResponse TranslationToGetTranslationResponse(Translation translation)
        {
            return new GetTranslationResponse
            {
                Id = translation.Id,
                OriginalText = translation.OriginalText,
                TranslatedText = translation.TranslatedText,
                SourceLanguage = translation.SourceLanguage,
                TargetLanguage = translation.TargetLanguage
            };
        }
    }
}