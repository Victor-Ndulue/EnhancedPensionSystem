using EnhancedPensionSystem_Application.Helpers.DTOs.Configs;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.Services.Implementations;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using EnhancedPensionSystem_Domain.Models;
using EnhancedPensionSystem_Infrastructure.Repository.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EnhancedPensionSystem_Application.UnitOfWork.Implementations;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<IAuthenticationService> _authenticationService;
    private readonly Lazy<IMemberService> _memberService;
    private readonly Lazy<IEmployerService> _employerService;
    private readonly Lazy<IContributionService> _contributionService;
    private readonly Lazy<ITransactionService> _transactionService;
    private readonly Lazy<IBenefitEligibilityService> _benefitEligibilityService;
    private readonly Lazy<INotificationService> _notificationService;
    private readonly Lazy<IEmailService> _emailService;
    private readonly Lazy<IBackgroundJobService> _backgroundJobService;

    public ServiceManager
        (
            IGenericRepository<Member> memberRepo,IGenericRepository<Employer> employerRepo,UserManager<AppUser> userManager,
            IGenericRepository<Contribution> contributionRepo, IGenericRepository<Transaction> transactionRepo, 
            IGenericRepository<BenefitEligibility> benefitEligibilityRepo,
            IGenericRepository<Notification> notificationRepo, IOptions<EmailConfig> emailConfig
        )
    {
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService());
        _memberService = new Lazy<IMemberService>(() => new MemberService(memberRepo, userManager, this));
        _employerService = new Lazy<IEmployerService>(() => new EmployerService(employerRepo, userManager, this));
        _contributionService = new Lazy<IContributionService>(()=> new ContributionService(contributionRepo, this));
        _transactionService = new Lazy<ITransactionService>(()=> new TransactionService(transactionRepo));
        _benefitEligibilityService = new Lazy<IBenefitEligibilityService>(()=> new BenefitEligibilityService(benefitEligibilityRepo, this));
        _notificationService = new Lazy<INotificationService>(() => new NotificationService(notificationRepo)); ;
        _emailService = new Lazy<IEmailService>(() => new EmailService(emailConfig)); ;
        _backgroundJobService = new Lazy<IBackgroundJobService>(() => new BackgroundJobService(contributionRepo, this)); ;
    }

    public IAuthenticationService AuthenticationService => _authenticationService.Value;

    public IMemberService MemberService => _memberService.Value;

    public IEmployerService EmployerService => _employerService.Value;

    public IContributionService ContributionService => _contributionService.Value;

    public ITransactionService TransactionService => _transactionService.Value;

    public IBenefitEligibilityService BenefitEligibilityService => _benefitEligibilityService.Value;

    public INotificationService NotificationService => _notificationService.Value;

    public IEmailService EmailService => _emailService.Value;

    public IBackgroundJobService BackgroundJobService => _backgroundJobService.Value;
}
