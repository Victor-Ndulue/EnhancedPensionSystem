namespace EnhancedPensionSystem_Application.Services.Abstractions;

public interface IBackgroundJobService
{
    Task ValidateContributionsAsync();
    Task CheckBenefitEligibilityAsync();
    Task CalculateInterestAsync();
    Task SendNotificationsAsync();
}
