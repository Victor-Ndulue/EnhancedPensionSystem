using FluentValidation;
using System.Text.RegularExpressions;

namespace EnhancedPensionSystem_Application.Helpers.DTOValidations;

public static class SharedDtoValidation
{
    public static IRuleBuilderOptions<T, DateTime?> ValidOptionalDateTime<T>(this IRuleBuilder<T, DateTime?> ruleBuilder)
    {
        return ruleBuilder
        .Must(date => !date.HasValue || IsValidDateFormat(date.Value))
        .WithMessage("Please enter a valid date in the format yyyy-MM-ddTHH:mm:ss")
        .Must(date => !date.HasValue || date <= DateTime.UtcNow.AddHours(1))
        .WithMessage("The date cannot be in the future");
    }
    public static IRuleBuilderOptions<T, TProperty> RequiredField<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("This field cannot be empty.");
    }
    public static IRuleBuilder<T, string?> ValidOptionalEmailAddress<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (ruleBuilder.Equals(null))
        {
            return ruleBuilder.Empty();
        }
        return ruleBuilder.Matches(pattern).WithMessage("Please enter a valid email address");
    }
    public static IRuleBuilder<T, string?> ValidOptionalPhoneNumericDigits<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        string digitPattern = @"^\d{6,14}$";
        string validationMessage = "Please enter a valid phone number with 6 to 14 digits.";

        return ruleBuilder
           .Must(inputDigit =>
           {
               if (string.IsNullOrWhiteSpace(inputDigit))
               {
                   return true;
               }

               return Regex.IsMatch(inputDigit, digitPattern);
           })
           .WithMessage(validationMessage);
    }
    private static bool IsValidDateFormat(DateTime date)
    {
        DateTime parsedDate;
        return DateTime.TryParseExact(date.ToString("yyyy-MM-ddTHH:mm:ss"),
            "yyyy-MM-ddTHH:mm:ss", null, System.Globalization.DateTimeStyles.None, out parsedDate);
    }
}
