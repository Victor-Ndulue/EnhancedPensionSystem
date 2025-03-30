using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Application.Helpers.DTOs.Responses;

public record TransactionResponse(
    string id,
    string memberID,
    string contributionID,
    decimal amount,
    ContributionType contributionType,
    TransactionStatus status,
    DateTime createdAt
);
