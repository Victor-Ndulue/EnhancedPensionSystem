using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using EnhancedPensionSystem_Domain.Enums;
using EnhancedPensionSystem_Domain.Models;
using EnhancedPensionSystem_Infrastructure.Repository.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnhancedPensionSystem_Application.Services.Implementations;

public sealed class MemberService : IMemberService
{
    private readonly IGenericRepository<Member> _memberRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IEmployerService _employerService;

    public MemberService(IGenericRepository<Member> memberRepository, UserManager<AppUser> userManager, 
        IServiceManager serviceManager)
    {
        _memberRepository = memberRepository;
        _userManager = userManager;
        _employerService = serviceManager.EmployerService;
    }

    public async Task<StandardResponse<string>>
        RegisterMemberAsync(CreateMemberParams createMemberParams)
    {
        var employerExists = await _employerService.ConfirmEmployerExistsAsync(createMemberParams.employerId);
        if (!employerExists)
        {
            string errorMsg = "Member does not match any employer. Confirm employer Id";
            return StandardResponse<string>.Failed(errorMsg);
        }
        // Validate Age Restriction (18 - 70 years)
        int age = DateTime.UtcNow.Year - createMemberParams.dateOfBirth.Value.Year;
        if (age < 18 || age > 70)
        {
            string errorMsg = "Member age must be between 18 and 70 years.";
            return StandardResponse<string>.Failed(null, errorMsg);
        }

        // Check if Email Already Exists
        bool emailExists = await EmailExistsAsync(createMemberParams.userEmail!);
        if (emailExists)
        {
            string errorMsg = "Email is already registered.";
            return StandardResponse<string>.Failed(null, errorMsg);
        }

        // Save to Identity & DB
        var member = new Member
        {
            FirstName = createMemberParams.FirstName,
            LastName = createMemberParams.LastName,
            Email = createMemberParams.userEmail,
            UserName = createMemberParams.userEmail,
            PhoneNumber = createMemberParams.phoneNumber,
            EmployerId = createMemberParams.employerId,
            DateOfBirth = createMemberParams.dateOfBirth.Value,
            AppUserType = AppUserType.Member
        };
        string defaultPassword = GenerateRandomPassword();
        var createUserResult = await _userManager.CreateAsync(member, defaultPassword);
        if (!createUserResult.Succeeded)
        {
            string errorMsg = $"Failed to register member. {createUserResult.Errors.FirstOrDefault()}";
            return StandardResponse<string>.Failed(null, errorMsg);
        }

        //Send Email to User with default password

        string successMsg = "Member successfully registered. Kindly check mail for other details.";
        return StandardResponse<string>.Accepted(data: successMsg);
    }

    public async Task<StandardResponse<IEnumerable<MemberResponse>>>
        GetAllMembersAsync()
    {
        var members = await _memberRepository.GetAllNonDeleted()
            .Include(mber => mber.Employer)
            .Select(mbers => new MemberResponse(
                mbers.Id, mbers.FirstName, mbers.LastName,
                mbers.Email, mbers.PhoneNumber,
                mbers.Employer!.CompanyName, mbers.DateOfBirth
            ))
            .AsNoTracking().ToListAsync();
        return StandardResponse<IEnumerable<MemberResponse>>.Success(members);
    }

    public async Task<StandardResponse<IEnumerable<MemberResponse>>>
        GetEmployerMembersAsync(string employerId)
    {
        var members = await _memberRepository.GetNonDeletedByCondition(mber => mber.EmployerId == employerId)
            .Include(mber => mber.Employer)
            .Select(mber => new MemberResponse(
                mber.Id, mber.FirstName, mber.LastName,
                mber.Email, mber.PhoneNumber,
                mber.Employer!.CompanyName, mber.DateOfBirth
            )).AsNoTracking().ToListAsync();
        return StandardResponse<IEnumerable<MemberResponse>>.Success(members);
    }

    public async Task<StandardResponse<MemberResponse>?> GetMemberByIdAsync(string memberId)
    {
        var member = await _memberRepository.GetNonDeletedByCondition(mber => mber.Id == memberId)
            .Include(mber => mber.Employer)
            .Select(mber => new MemberResponse(
                mber.Id, mber.FirstName, mber.LastName,
                mber.Email, mber.PhoneNumber,
                mber.Employer!.CompanyName, mber.DateOfBirth
            )).AsNoTracking().FirstOrDefaultAsync();

        return StandardResponse<MemberResponse>.Success(member);
    }

    public async Task<StandardResponse<string>> SoftDeleteMemberAsync(string memberId)
    {
        var memberToDelete = _memberRepository.GetNonDeletedByCondition(mber => mber.Id == memberId)
            .FirstOrDefault();
        if (memberToDelete is null)
        {
            string errorMsg = "No data record found";
            return StandardResponse<string>.Failed(errorMsg);
        }
        _memberRepository.SoftDelete(memberToDelete);
        await _memberRepository.SaveChangesAsync();
        string successMsg = "Record deleted successfuly";
        return StandardResponse<string>.Success(data: successMsg, statusCode: 201);
    }

    public async Task<StandardResponse<string>> UpdateMemberAsync(UpdateMemberParams memberUpdateParams)
    {
        var memberToUpdate = _memberRepository.GetNonDeletedByCondition(mber => mber.Id == memberUpdateParams.MemberId)
            .FirstOrDefault();
        if(memberToUpdate is null)
        {
            string errorMsg = "No data record found for member";
            return StandardResponse<String>.Failed(errorMsg);
        }
        if (ConfirmStringAdded(memberUpdateParams.FirstName))
        {
            memberToUpdate.FirstName = memberUpdateParams.FirstName;
        }
        if (ConfirmStringAdded(memberUpdateParams.LastName))
        {
            memberToUpdate.LastName = memberUpdateParams.LastName;
        }
        if (ConfirmStringAdded(memberUpdateParams.userEmail))
        {            
            // Check if Email Already Exists
            bool emailExists = await EmailExistsAsync(memberUpdateParams.userEmail!);
            if (emailExists)
            {
                string errorMsg = "Email is already registered.";
                return StandardResponse<string>.Failed(null, errorMsg);
            }
            memberToUpdate.Email = memberUpdateParams.userEmail;
        }
        if (ConfirmStringAdded(memberUpdateParams.phoneNumber))
        {
            memberToUpdate.PhoneNumber = memberUpdateParams.phoneNumber;
        }
        if (memberUpdateParams.dateOfBirth.HasValue) 
        {
            // Validate Age Restriction (18 - 70 years)
            int age = DateTime.UtcNow.Year - memberUpdateParams.dateOfBirth.Value.Year;
            if (age < 18 || age > 70)
            {
                string errorMsg = "Member age must be between 18 and 70 years.";
                return StandardResponse<string>.Failed(null, errorMsg);
            }
            memberToUpdate.DateOfBirth = memberUpdateParams.dateOfBirth;
        }
        await _memberRepository.SaveChangesAsync();

        string? successMsg = "User updated Successfully";
        return StandardResponse<string>.Success(successMsg);
    }

    public async Task<bool> ConfirmMemberExists(string? memberId)
    {
        return await _memberRepository.ExistsByConditionAsync(m => m.Id == memberId);       
    }

    private async Task<bool> EmailExistsAsync(string email)
    {
        return await _memberRepository.ExistsByConditionAsync(m => m.Email == email);
    }

    private string GenerateRandomPassword()
    {
        return Guid.NewGuid().ToString().Substring(0, 8);
    }

    private bool ConfirmStringAdded(string? stringValue)
    {
        return !string.IsNullOrWhiteSpace(stringValue);
    }
}
