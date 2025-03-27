using FluentValidation;

namespace EnhancedPensionSystem_Application.Helpers.DTOValidations;

public static class SharedDtoValidation
{
    public static IRuleBuilderOptions<T, TProperty> RequiredField<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("This field cannot be empty.");
    }
}
