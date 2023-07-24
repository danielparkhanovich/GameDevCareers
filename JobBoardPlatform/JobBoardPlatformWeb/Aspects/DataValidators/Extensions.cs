using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JobBoardPlatform.PL.Aspects.DataValidators
{
    public static class Extensions
    {
        public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState, string? prefix = null)
        {
            foreach (var error in result.Errors)
            {
                if (prefix != null)
                {
                    error.PropertyName = $"{prefix}.{error.PropertyName}";
                }
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
