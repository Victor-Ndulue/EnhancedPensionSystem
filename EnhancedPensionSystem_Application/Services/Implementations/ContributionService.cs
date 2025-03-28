using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using EnhancedPensionSystem_Domain.Enums;
using EnhancedPensionSystem_Domain.Models;
using EnhancedPensionSystem_Infrastructure.Repository.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EnhancedPensionSystem_Application.Services.Implementations;

public sealed class ContributionService : IContributionService
{
    private readonly IGenericRepository<Contribution> _contributionRepository;
    private readonly IMemberService _memberService;
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;

    public ContributionService(IGenericRepository<Contribution> contributionRepository, IServiceManager serviceManager)
    {
        _contributionRepository = contributionRepository;
        _memberService = serviceManager.MemberService;
        _transactionService = serviceManager.TransactionService;
        _notificationService = serviceManager.NotificationService;
    }

    public async Task<StandardResponse<string>> 
        AddContributionAsync(AddContributionParams contributionParams)
    {
        // Validate Member Exists
        var memberExists = await _memberService.ConfirmMemberExists(contributionParams.MemberId);
        if (!memberExists)
        {
            string errorMsg = "Member not found";
            return StandardResponse<string>.Failed(errorMsg);
        }

        // Validate Contribution Amount
        if (contributionParams.Amount <= 0)
        {
            string? errorMsg = "Contribution amount must be greater than 0.00.";
            return StandardResponse<string>.Failed(errorMsg);
        }

        // Check if Monthly Contribution Already Exists
        if (contributionParams.Type == ContributionType.Monthly)
        {
            var monthExists = await _contributionRepository.ExistsByConditionAsync(c =>
                c.MemberId == contributionParams.MemberId &&
                c.ContributionDate.Year == contributionParams.ContributionDate!.Value.Year &&
                c.ContributionDate.Month == contributionParams.ContributionDate.Value.Month &&
                c.ContributionType == ContributionType.Monthly);

            if (monthExists)
            {
                string errorMsg = "A monthly contribution for this period already exists.";
                return StandardResponse<string>.Failed(errorMsg);
            }
        }
        // Save Contribution
        var contribution = new Contribution
        {
            MemberId = contributionParams.MemberId,
            Amount = contributionParams.Amount!.Value,
            ContributionDate = contributionParams.ContributionDate!.Value,
            ContributionType = contributionParams.Type
        };
        await _contributionRepository.AddAsync(contribution);

        string notificationMsg = $"Contribution for the month {contribution.ContributionDate.Month} received.";
        var notification = new NotificationParams(contributionParams.MemberId, notificationMsg, NotificationType.ContributionReceived);
        await _notificationService.CreateNotificationAsync(notification);

        await _transactionService.AddTransactionAsync(new AddTransactionParams
            (
                contributionParams.MemberId,
                contribution.Id,
                contributionParams.Amount.Value,
                contributionParams.Type,
                TransactionStatus.Successful
            )); //Contribustion persisted on Db via one save at the Transaction Servoce

        return StandardResponse<string>.Success("Contribution successfully added.");
    }

    public async Task<StandardResponse<IEnumerable<ContributionResponse>>> 
        GetAllContributionsAsync()
    {
        var contributions = await _contributionRepository.GetAllNonDeleted()
            .Select(c => new ContributionResponse(c.Id, c.MemberId, c.Amount, c.ContributionDate, c.ContributionType))
            .AsNoTracking().ToListAsync();

        return StandardResponse<IEnumerable<ContributionResponse>>.Success(contributions);
    }

    public async Task<StandardResponse<IEnumerable<ContributionResponse>>> 
        GetMemberContributionsAsync(string memberId)
    {
        var contributions = await _contributionRepository.GetNonDeletedByCondition(c => c.MemberId == memberId)
            .Select(c => new ContributionResponse(c.Id, c.MemberId, c.Amount, c.ContributionDate, c.ContributionType))
            .AsNoTracking().ToListAsync();

        return StandardResponse<IEnumerable<ContributionResponse>>.Success(contributions);
    }

    public async Task<StandardResponse<TotalContributionResponse>> 
        GetTotalContributionsAsync(string memberId)
    {
        var contributions = await _contributionRepository.GetNonDeletedByCondition(c => c.MemberId == memberId)
            .GroupBy(c => c.ContributionType)
            .Select(group => new ContributionBreakdown
            (
                group.Key.ToString(),
                group.Sum(c => c.Amount)
            ))
            .ToListAsync();

        var totalAmount = contributions.Sum(c => c.totalAmount);

        var response = new TotalContributionResponse
        (
            totalContributions: totalAmount,
            breakdown: contributions
        );

        return StandardResponse<TotalContributionResponse>.Success(response);
    }
}
