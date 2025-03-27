using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Domain.Models;

public class Contribution: BaseEntity
{
    public Guid MemberId { get; private set; }
    public Member? Member { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime ContributionDate { get; private set; }
    public ContributionType Type { get; private set; }
}
