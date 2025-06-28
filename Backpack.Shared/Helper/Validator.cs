using RecursiveDataAnnotationsValidation;
using System.ComponentModel.DataAnnotations;

namespace Backpack.Shared.Helper;
public static class Validator
{
    public static void Validate(object obj)
    {
        var validator = new RecursiveDataAnnotationValidator();
        var results = new List<ValidationResult>();

        if (!validator.TryValidateObjectRecursive(obj, results))
        {
            var errorMessages = string.Join(Environment.NewLine, results.ConvertAll(r => r.ErrorMessage));
            throw new ValidationException($"Configuration validation failed:{Environment.NewLine}{errorMessages}");
        }
    }
}
