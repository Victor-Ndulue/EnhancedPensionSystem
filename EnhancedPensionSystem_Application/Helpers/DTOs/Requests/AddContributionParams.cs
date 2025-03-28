using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

public record AddContributionParams
    (string MemberId, decimal? Amount, 
    DateTime? ContributionDate, ContributionType Type);
