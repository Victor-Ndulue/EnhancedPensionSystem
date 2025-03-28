using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

public record AddTransactionParams(
    string MemberId,
    string ContributionId,
    decimal Amount,
    ContributionType ContributionType,
    TransactionStatus Status
);
