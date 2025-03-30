using EnhancedPensionSystem_Application.Services.Abstractions;

namespace EnhancedPensionSystem_Application.UnitOfWork.Abstraction;

public interface IServiceManager
{
    IAuthenticationService AuthenticationService { get; }
    IMemberService MemberService { get; }
    IEmployerService EmployerService { get; }
    IContributionService ContributionService { get; }
    ITransactionService TransactionService { get; }
    IBenefitEligibilityService BenefitEligibilityService { get; }
    INotificationService NotificationService { get; }
    IEmailService EmailService { get; }
    IBackgroundJobService BackgroundJobService { get; }
}