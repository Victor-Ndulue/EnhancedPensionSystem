using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using EnhancedPensionSystem_Domain.Enums;
using EnhancedPensionSystem_Domain.Models;
using EnhancedPensionSystem_Infrastructure.Repository.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EnhancedPensionSystem_Application.Services.Implementations;

public sealed class BackgroundJobService : IBackgroundJobService
{
    private readonly IGenericRepository<Contribution> _contributionRepo;
    private readonly IBenefitEligibilityService _benefitEligibilityService;
    private readonly INotificationService _notificationService;
    private readonly decimal interestRate = 0.05m;

    public BackgroundJobService(IGenericRepository<Contribution> contributionRepo, IServiceManager serviceManager)
    {
        _contributionRepo = contributionRepo;
        _benefitEligibilityService = serviceManager.BenefitEligibilityService;
        _notificationService = serviceManager.NotificationService;
    }

    /// <summary>
    /// Validates pending contributions asynchronously
    /// </summary>
    public async Task ValidateContributionsAsync()
    {
        var invalidContributions =  _contributionRepo.GetNonDeletedByCondition(c=>c.Amount<=0);
        if (!invalidContributions.Any())
        {
            return;
        }

        foreach (var contribution in invalidContributions)
        {
            await _notificationService.CreateNotificationAsync(new NotificationParams(
                contribution.MemberId,
                $"Your contribution on {contribution.ContributionDate} is invalid.",
                NotificationType.ContributionError
            ));
        }
        _contributionRepo.SoftDeleteRange(invalidContributions);
        await _contributionRepo.SaveChangesAsync();
    }

    /// <summary>
    /// Checks and updates benefit eligibility asynchronously
    /// </summary>
    public async Task CheckBenefitEligibilityAsync()
    {
        await _benefitEligibilityService.UpdateBenefitEligibilityAsync();
    }

    /// <summary>
    /// Calculates interest on contributions asynchronously
    /// </summary>
    public async Task CalculateInterestAsync()
    {

        // Fetch all contributions that are eligible for interest
        var contributionsResponse = await _contributionRepo.GetAllNonDeleted().ToListAsync();
        if (!contributionsResponse.Any())
        {
            return;
        }

        // Process interest calculations
        var updatedContributions = new List<Contribution>();

        foreach (var contribution in contributionsResponse)
        {
            decimal interest = contribution.Amount * interestRate;
            contribution.Amount += interest;
            updatedContributions.Add(contribution);
        }

        // Bulk update contributions with the new amounts
        _contributionRepo.UpdateRange(updatedContributions);
        await _contributionRepo.SaveChangesAsync();
    }

    /// <summary>
    /// Sends pending notifications asynchronously
    /// </summary>
    public async Task SendNotificationsAsync()
    {
        return;
    }
}
