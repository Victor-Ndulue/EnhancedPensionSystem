using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;

namespace EnhancedPensionSystem_Application.Services.Abstractions;

public interface IBenefitEligibilityService
{
    Task UpdateBenefitEligibilityAsync();
    Task<StandardResponse<BenefitEligibilityResponse>> CheckMemberEligibilityAsync(string memberId);
    Task<StandardResponse<IEnumerable<BenefitEligibilityResponse>>> GetAllEligibleMembersAsync();
}
