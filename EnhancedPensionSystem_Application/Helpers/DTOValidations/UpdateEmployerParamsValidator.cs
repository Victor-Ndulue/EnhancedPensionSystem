using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using FluentValidation;

namespace EnhancedPensionSystem_Application.Helpers.DTOValidations;

public class UpdateEmployerParamsValidator : AbstractValidator<UpdateEmployerParams>
{
    public UpdateEmployerParamsValidator()
    {
        RuleFor(x => x.employerId)
            .RequiredField();

        RuleFor(x => x.email)
            .ValidOptionalEmailAddress();

        RuleFor(x => x.phoneNumber)
            .ValidOptionalPhoneNumericDigits();
    }
}
