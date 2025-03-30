using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using FluentValidation;

namespace EnhancedPensionSystem_Application.Helpers.DTOValidations;

public class AddContributionParamsValidator : AbstractValidator<AddContributionParams>
{
    public AddContributionParamsValidator()
    {
        RuleFor(x => x.memberId)
            .RequiredField();

        RuleFor(x => x.amount)
            .NotNull().WithMessage("Contribution amount is required.")
            .GreaterThan(0).WithMessage("Contribution amount must be greater than 0.");

        RuleFor(x => x.contributionDate)
            .ValidOptionalDateTime();
    }
}
