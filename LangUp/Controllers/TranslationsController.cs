using LangUp.Database;
using LangUp.DTOs.Translation;
using LangUp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LangUp.Controllers;

public class TranslationsController : BaseController
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<TranslationsController> _logger;

    public TranslationsController(AppDbContext dbContext, ILogger<TranslationsController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    /// <summary>
    /// Gets a translation by ID.
    /// </summary>
    /// <param name="id">The ID of the translation.</param>
    /// <returns>The single translation record.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(GetTranslationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTranslationById(int id)
    {
        var translation = await _dbContext.Translations.SingleOrDefaultAsync(e => e.Id == id);
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
    /// <param name="translationRequest">The translation to be created.</param>
    /// <returns>A link to the translation that was created.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(GetTranslationResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateTranslation([FromBody] CreateTranslationRequest translationRequest)
    {
        var newTranslation = new Translation
        {
            OriginalText = translationRequest.OriginalText,
            TranslatedText = translationRequest.TranslatedText,
            SourceLanguage = translationRequest.SourceLanguage,
            TargetLanguage = translationRequest.TargetLanguage
        };

        _dbContext.Translations.Add(newTranslation);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTranslationById), new { id = newTranslation.Id }, newTranslation);
    }
    
    private static GetTranslationResponse TranslationToGetTranslationResponse(Translation translation)
    {
        return new GetTranslationResponse
        {
            OriginalText = translation.OriginalText,
            TranslatedText = translation.TranslatedText,
            SourceLanguage = translation.SourceLanguage,
            TargetLanguage = translation.TargetLanguage
        };
    }
}