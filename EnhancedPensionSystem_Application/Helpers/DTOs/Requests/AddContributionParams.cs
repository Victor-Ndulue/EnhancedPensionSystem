using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

/// <summary>
/// Represents the parameters required to add a new contribution.
/// </summary>
/// <param name="memberId">The unique identifier of the contributing member.</param>
/// <param name="amount">The amount of the contribution (must be greater than 0).</param>
/// <param name="contributionDate">The date of the contribution.</param>
/// <param name="type">The type of the contribution.</param>
public record AddContributionParams
    (string memberId, decimal? amount, 
    DateTime? contributionDate, ContributionType type);
