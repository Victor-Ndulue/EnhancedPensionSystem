namespace EnhancedPensionSystem_Application.Helpers.DTOs.Responses;

public record TotalContributionResponse
(
    decimal? totalContributions, 
    IEnumerable<ContributionBreakdown>? breakdown
);

public record ContributionBreakdown
(
    string? contributionType,
    decimal? totalAmount
);
