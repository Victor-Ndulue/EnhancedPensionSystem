using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.Services.Implementations;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using EnhancedPensionSystem_Domain.Models;
using EnhancedPensionSystem_Infrastructure.Repository.Implementations;
using Microsoft.AspNetCore.Identity;

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
            IGenericRepository<Member> memberRepo,IGenericRepository<Employer> employerRepo,UserManager<Member> memberManager,
            IGenericRepository<Contribution> contributionRepo, UserManager<Employer> employerManager
        )
    {
        _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService());
        _memberService = new Lazy<IMemberService>(() => new MemberService(memberRepo, memberManager, this));
        _employerService = new Lazy<IEmployerService>(() => new EmployerService(employerRepo, employerManager));
        _contributionService = new Lazy<IContributionService>(()=> new ContributionService(contributionRepo, this));
        _transactionService = new Lazy<ITransactionService>(()=> new TransactionService());
        _benefitEligibilityService = new Lazy<IBenefitEligibilityService>(()=> new BenefitEligibilityService());
        _notificationService = new Lazy<INotificationService>(() => new NotificationService()); ;
        _emailService = new Lazy<IEmailService>(() => new EmailService()); ;
        _backgroundJobService = new Lazy<IBackgroundJobService>(() => new BackgroundJobService()); ;
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
