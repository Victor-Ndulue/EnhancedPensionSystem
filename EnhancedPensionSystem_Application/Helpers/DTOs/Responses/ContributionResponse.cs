using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Application.Helpers.DTOs.Responses;

public record ContributionResponse
    (string? id, string? memberId, decimal amount, 
    DateTime? contributionDate, ContributionType contributionType);
