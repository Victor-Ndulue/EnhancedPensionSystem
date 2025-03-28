using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Domain.Models;

public class Transaction:BaseEntity
{
    public string? ContributionId { get; private set; }
    public Contribution? Contribution { get; private set; }
    public decimal Amount { get; private set; }
    public TransactionStatus Status { get; private set; }
    public string? FailureReason { get; private set; }
}
