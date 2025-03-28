using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Domain.Models;

public class Contribution: BaseEntity
{
    public string? MemberId { get; set; }
    public Member? Member { get; set; }
    public decimal Amount { get; set; }
    public DateTime ContributionDate { get; set; }
    public ContributionType ContributionType { get; set; }
}
