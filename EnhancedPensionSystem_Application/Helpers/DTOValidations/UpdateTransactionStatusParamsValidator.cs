using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using FluentValidation;

namespace EnhancedPensionSystem_Application.Helpers.DTOValidations;

public class UpdateTransactionStatusParamsValidator : AbstractValidator<UpdateTransactionStatusParams>
{
    public UpdateTransactionStatusParamsValidator()
    {
        RuleFor(x => x.transactionId)
            .RequiredField();

        RuleFor(x => x.status)
            .IsInEnum().WithMessage("Invalid transaction status.");
    }
}
