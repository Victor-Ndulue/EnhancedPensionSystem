using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;

namespace EnhancedPensionSystem_Application.Services.Abstractions;

public interface IMemberService
{
    Task<StandardResponse<MemberResponse>> RegisterMemberAsync(CreateMemberParams createMemberParams);
    Task<StandardResponse<MemberResponse>?> GetMemberByIdAsync(Guid memberId);
    Task<StandardResponse<IEnumerable<MemberResponse>>> GetAllMembersAsync();
    Task<StandardResponse<string>> UpdateMemberAsync(UpdateMemberParams member);
    Task<StandardResponse<string>> SoftDeleteMemberAsync(MemberResponse memberId);
}
