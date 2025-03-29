using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using FluentValidation;

namespace EnhancedPensionSystem_Application.Helpers.DTOValidations;

public class UpdateMemberParamsValidator : AbstractValidator<UpdateMemberParams>
{
    public UpdateMemberParamsValidator()
    {
        RuleFor(x => x.MemberId)
            .RequiredField();

        RuleFor(x => x.userEmail)
            .ValidOptionalEmailAddress();

        RuleFor(x => x.phoneNumber)
            .ValidOptionalPhoneNumericDigits();

        RuleFor(x => x.dateOfBirth)
            .ValidOptionalDateTime()
            .Must(date => !date.HasValue || (DateTime.UtcNow.Year - date.Value.Year) is >= 18 and <= 70)
            .WithMessage("Member must be between 18 and 70 years old.");
    }
}
