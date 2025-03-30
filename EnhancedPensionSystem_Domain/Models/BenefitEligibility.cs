namespace EnhancedPensionSystem_Domain.Models;

public class BenefitEligibility : BaseEntity
{
    public string? MemberId { get; set; }
    public Member? Member { get; set; }
    public bool IsEligible { get; set; } 
    public DateTime EligibilityDate { get; set; }
}
