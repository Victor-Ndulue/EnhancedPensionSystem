using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using EnhancedPensionSystem_Domain.Models;
using EnhancedPensionSystem_Infrastructure.Repository.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EnhancedPensionSystem_Application.Services.Implementations;

public sealed class BenefitEligibilityService : IBenefitEligibilityService
{
    private readonly IGenericRepository<BenefitEligibility> _benefitEligibilityRepository;
    private readonly IContributionService _contributionService;
    private readonly IMemberService _memberService;
    private readonly int _minimumContributionMonths = 60; // Configurable

    public BenefitEligibilityService(
        IGenericRepository<BenefitEligibility> benefitEligibilityRepository,
        IServiceManager serviceManager)
    {
        _benefitEligibilityRepository = benefitEligibilityRepository;
        _contributionService = serviceManager.ContributionService;
        _memberService = serviceManager.MemberService;
    }

    public async Task UpdateBenefitEligibilityAsync()
    {
        // Fetch all contributions
        var contributionsResponse = await _contributionService.GetAllContributionsAsync();
        if (!contributionsResponse.Data.Any()) return;

        var existingMembers = await _benefitEligibilityRepository.GetAllNonDeleted().ToListAsync();
        var existingMemberIds = existingMembers.Select(e => e.MemberId).ToHashSet();


        // Calculate months contributed per member
        var contributionsByMember = contributionsResponse.Data
            .GroupBy(c => c.memberId)
            .Select(g => new
            {
                MemberId = g.Key,
                MonthsContributed = g
                    .Select(c => new { c.contributionDate!.Value.Year, c.contributionDate.Value.Month })
                    .Distinct()
                    .Count()
            })
            .Where(c => !existingMemberIds.Contains(c.MemberId))
            .ToList();

        if (!contributionsByMember.Any()) return;

        // Prepare new eligibility records
        var benefitUpdates = contributionsByMember
            .Select(c => new BenefitEligibility
            {
                MemberId = c.MemberId,
                IsEligible = c.MonthsContributed >= _minimumContributionMonths,
                EligibilityDate = DateTime.UtcNow
            })
            .ToList();

        // Bulk insert new eligibility records
        await _benefitEligibilityRepository.AddRangeAsync(benefitUpdates);
        await _benefitEligibilityRepository.SaveChangesAsync();
    }

    public async Task<StandardResponse<BenefitEligibilityResponse>> CheckMemberEligibilityAsync(string memberId)
    {
        var memberEligibility = await _benefitEligibilityRepository
            .GetNonDeletedByCondition(e => e.MemberId == memberId)
            .FirstOrDefaultAsync();

        var response = memberEligibility is not null
            ? new BenefitEligibilityResponse(memberId, true, memberEligibility.EligibilityDate, "Member is eligible for benefits.")
            : new BenefitEligibilityResponse(memberId, false, null, "Not enough contributions made.");

        return StandardResponse<BenefitEligibilityResponse>.Success(response);
    }

    public async Task<StandardResponse<IEnumerable<BenefitEligibilityResponse>>> GetAllEligibleMembersAsync()
    {
        var eligibleMembers = await _benefitEligibilityRepository.GetAllNonDeleted()
            .Where(e => e.IsEligible) // Only return eligible members
            .Select(e => new BenefitEligibilityResponse(e.MemberId, e.IsEligible, e.EligibilityDate, "Member is eligible for benefits."))
            .AsNoTracking()
            .ToListAsync();

        return StandardResponse<IEnumerable<BenefitEligibilityResponse>>.Success(eligibleMembers);
    }
}