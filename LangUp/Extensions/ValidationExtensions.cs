using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LangUp.Extensions;

public static class ValidationExtensions
{
    public static ModelStateDictionary ToModelStateDictionary(this ValidationResult validationResult)
    {
        var modelState = new ModelStateDictionary();

        foreach (var error in validationResult.Errors)
        {
            modelState.AddModelError(error.PropertyName, error.ErrorMessage);
        }

        return modelState;
    }
}