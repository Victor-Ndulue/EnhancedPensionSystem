using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;

namespace EnhancedPensionSystem_Application.Services.Abstractions;

public interface IContributionService
{
    Task<StandardResponse<string>> AddContributionAsync(AddContributionParams contributionParams);
    Task<StandardResponse<IEnumerable<ContributionResponse>>> GetMemberContributionsAsync(string memberId);
    Task<StandardResponse<TotalContributionResponse>> GetTotalContributionsAsync(string memberId);
    Task<StandardResponse<IEnumerable<ContributionResponse>>> GetAllContributionsAsync();
}
