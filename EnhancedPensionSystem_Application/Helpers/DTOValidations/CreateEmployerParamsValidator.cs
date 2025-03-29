using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using FluentValidation;

namespace EnhancedPensionSystem_Application.Helpers.DTOValidations;

public class CreateEmployerParamsValidator : AbstractValidator<CreateEmployerParams>
{
    public CreateEmployerParamsValidator()
    {
        RuleFor(x => x.companyName)
            .RequiredField();

        RuleFor(x => x.registrationNumber)
            .RequiredField();

        RuleFor(x => x.email)
            .ValidOptionalEmailAddress();

        RuleFor(x => x.phoneNumber)
            .ValidOptionalPhoneNumericDigits();
    }
}
