using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Domain.Models;

public class Transaction : BaseEntity
{
    public string? ContributionId { get; set; }
    public Contribution? Contribution { get; set; }
    public decimal Amount { get; set; }
    public TransactionStatus TransactionStatus { get; set; }
    public ContributionType ContributionType { get; set; }
    public string? FailureReason { get; set; }
    public string? MemberId { get; set; }
    public virtual Member? Member { get; set; }
}
