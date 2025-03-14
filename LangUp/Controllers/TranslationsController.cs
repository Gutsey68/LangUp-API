using System.Security.Claims;
using LangUp.DTOs.Translation;
using LangUp.Models;
using LangUp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LangUp.Controllers
{
    [Authorize]
    public class TranslationsController : BaseController
    {
        private readonly ITranslationService _translationService;
        
        public TranslationsController(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        /// <summary>
        /// Retrieves all translations.
        /// </summary>
        /// <param name="request">The request parameters for filtering translations.</param>
        /// <returns>A list of translations.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GetTranslationRequest>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(GetTranslationRequest), StatusCodes.Status200OK)]
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
        /// <param name="request">The request containing the translation details.</param>
        /// <returns>The created translation.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(GetTranslationRequest), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateTranslation(CreateTranslationRequest request)
        {
            var userIdClaim = User.FindFirstValue("UserId");
            
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                return Unauthorized(new { message = "User ID not found in token" });
            }
    
            request.UserId = userId;
    
            var translation = await _translationService.CreateTranslationAsync(request);
            return CreatedAtAction(nameof(GetTranslationById), new { id = translation.Id }, translation);
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

        private static GetTranslationRequest TranslationToGetTranslationResponse(Translation translation)
        {
            return new GetTranslationRequest
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