namespace EnhancedPensionSystem_Domain.Models;

public class BenefitEligibility : BaseEntity
{
    public Guid MemberId { get; private set; }
    public Member? Member { get; private set; }
    public bool IsEligible { get; private set; } = false;
    public DateTime EligibilityDate { get; private set; }
}
