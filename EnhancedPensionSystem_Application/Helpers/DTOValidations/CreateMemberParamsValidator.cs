using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using FluentValidation;

namespace EnhancedPensionSystem_Application.Helpers.DTOValidations;

public class CreateMemberParamsValidator : AbstractValidator<CreateMemberParams>
{
    public CreateMemberParamsValidator()
    {
        RuleFor(x => x.FirstName)
            .RequiredField();

        RuleFor(x => x.LastName)
            .RequiredField();

        RuleFor(x => x.userEmail)
            .ValidOptionalEmailAddress().RequiredField();

        RuleFor(x => x.phoneNumber)
            .ValidOptionalPhoneNumericDigits().RequiredField();

        RuleFor(x => x.dateOfBirth)
            .ValidOptionalDateTime().RequiredField()
            .Must(date => !date.HasValue || (DateTime.UtcNow.Year - date.Value.Year) is >= 18 and <= 70)
            .WithMessage("Member must be between 18 and 70 years old.");
    }
}
