namespace EnhancedPensionSystem_Application.Helpers.DTOs.Responses;

public record BenefitEligibilityResponse
(
    string? MemberId,
    bool? IsEligible,
    DateTime? elegibilityDate,
    string? Message
);
