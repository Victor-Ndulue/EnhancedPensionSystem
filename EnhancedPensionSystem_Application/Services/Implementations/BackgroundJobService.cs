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
    private readonly ITransactionService _transactionService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<BackgroundJobService> _logger;
    private readonly decimal interestRate = 0.05m;

    public BackgroundJobService(IGenericRepository<Contribution> contributionRepo, IServiceManager serviceManager,
        ILogger<BackgroundJobService> logger)
    {
        _contributionRepo = contributionRepo;
        _benefitEligibilityService = serviceManager.BenefitEligibilityService;
        _transactionService = serviceManager.TransactionService;
        _notificationService = serviceManager.NotificationService;
        _logger = logger;
    }

    /// <summary>
    /// Validates pending contributions asynchronously
    /// </summary>
    public async Task ValidateContributionsAsync()
    {
        _logger.LogInformation("Starting contribution validation...");

        var invalidContributions =  _contributionRepo.GetNonDeletedByCondition(c=>c.Amount<=0);
        if (!invalidContributions.Any())
        {
            _logger.LogInformation("No invalid contributions found.");
            return;
        }

        foreach (var contribution in invalidContributions)
        {
            _logger.LogWarning($"Invalid Contribution Found: {contribution.Id}");
            await _notificationService.CreateNotificationAsync(new NotificationParams(
                contribution.MemberId,
                $"Your contribution on {contribution.ContributionDate} is invalid.",
                NotificationType.ContributionError
            ));
        }
        _contributionRepo.SoftDeleteRange(invalidContributions);
        await _contributionRepo.SaveChangesAsync();
        _logger.LogInformation("Contribution validation completed.");
    }

    /// <summary>
    /// Checks and updates benefit eligibility asynchronously
    /// </summary>
    public async Task CheckBenefitEligibilityAsync()
    {
        _logger.LogInformation("Checking benefit eligibility...");
        await _benefitEligibilityService.UpdateBenefitEligibilityAsync();
        _logger.LogInformation("Benefit eligibility check completed.");
    }

    /// <summary>
    /// Calculates interest on contributions asynchronously
    /// </summary>
    public async Task CalculateInterestAsync()
    {
        _logger.LogInformation("Starting interest calculation for contributions...");

        // Fetch all contributions that are eligible for interest
        var contributionsResponse = await _contributionRepo.GetAllNonDeleted().ToListAsync();
        if (!contributionsResponse.Any())
        {
            _logger.LogInformation("No contributions found for interest calculation.");
            return;
        }

        // Process interest calculations
        var updatedContributions = new List<Contribution>();

        foreach (var contribution in contributionsResponse)
        {
            decimal interest = contribution.Amount * interestRate;
            contribution.Amount += interest;
            updatedContributions.Add(contribution);

            _logger.LogInformation($"Applied interest: {interest:C} to Contribution ID: {contribution.Id}");
        }

        // Bulk update contributions with the new amounts
        _contributionRepo.UpdateRange(updatedContributions);
        await _contributionRepo.SaveChangesAsync();
        _logger.LogInformation("Interest calculation completed for all contributions.");
    }

    /// <summary>
    /// Sends pending notifications asynchronously
    /// </summary>
    public async Task SendNotificationsAsync()
    {
        _logger.LogInformation("Sending notifications...");
        return
        _logger.LogInformation("Notifications sent.");
    }
}
