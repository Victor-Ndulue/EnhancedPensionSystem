using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Implementations;
using EnhancedPensionSystem_Domain.Models;
using EnhancedPensionSystem_Infrastructure.Repository.Implementations;
using Microsoft.AspNetCore.Identity;

namespace EnhancedPensionSystem_Application.Services.Implementations;

public sealed class MemberService : IMemberService
{
    private readonly IGenericRepository<Member> _memberRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly ServiceManager _serviceManager;

    public MemberService(IGenericRepository<Member> memberRepository, UserManager<AppUser> userManager, 
        ServiceManager serviceManager)
    {
        _memberRepository = memberRepository;
        _userManager = userManager;
        _serviceManager = serviceManager;
    }

    public async Task<StandardResponse<MemberResponse>> RegisterMemberAsync(CreateMemberParams createMemberParams)
    {
        // Validate Age Restriction (18 - 70 years)
        int age = DateTime.UtcNow.Year - createMemberParams.DateOfBirth.Value.Year;
        if (age < 18 || age > 70)
        {
            string errorMsg = "Member age must be between 18 and 70 years.";
            return StandardResponse<MemberResponse>.Failed(null, errorMsg);
        }

        // Check if Email Already Exists
        bool emailExists = await EmailExistsAsync(createMemberParams.userEmail!);
        if (emailExists)
        { 
           string errorMsg = "Email is already registered.";
            return StandardResponse<MemberResponse>.Failed(null, errorMsg);
        }

        // Save to Identity & DB
        var member = new Member
        {
            FirstName = createMemberParams.FirstName,
            LastName = createMemberParams.LastName,
            Email = createMemberParams.userEmail,
            PhoneNumber = createMemberParams.PhoneNumber,
            EmployerId = createMemberParams.employerId,
            DateOfBirth = createMemberParams.DateOfBirth.Value
        };
        string defaultPassword = GenerateRandomPassword();
        var createUserResult = await _userManager.CreateAsync(member, defaultPassword); 
        //Send Email to User with default password
        if (!createUserResult.Succeeded)
        {
            string errorMsg = $"Failed to register member. {createUserResult.Errors.FirstOrDefault()}";
            return StandardResponse<MemberResponse>.Failed(null, errorMsg);
        }
        await _memberRepository.AddAsync(member);
        await _memberRepository.SaveChangesAsync();
        string? employerName = await _serviceManager.EmployerService.GetEmployerNameByIdAsync(member.EmployerId!);
        var responseData = new MemberResponse(
            member.Id, member.FirstName, member.LastName,
            member.Email, member.PhoneNumber, employerName, member.DateOfBirth);
        return StandardResponse<MemberResponse>.Success(responseData);
    }

    public Task<StandardResponse<IEnumerable<MemberResponse>>> GetAllMembersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<StandardResponse<MemberResponse>?> GetMemberByIdAsync(Guid memberId)
    {
        throw new NotImplementedException();
    }

    public Task<StandardResponse<string>> SoftDeleteMemberAsync(MemberResponse memberId)
    {
        throw new NotImplementedException();
    }

    public Task<StandardResponse<string>> UpdateMemberAsync(UpdateMemberParams member)
    {
        throw new NotImplementedException();
    }

    private async Task<bool> EmailExistsAsync(string email)
    {
        return await _memberRepository.ExistsByConditionAsync(m => m.Email == email);
    }

    private string GenerateRandomPassword()
    {
        return Guid.NewGuid().ToString().Substring(0, 8);
    }
}
