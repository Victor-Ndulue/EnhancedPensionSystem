using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Domain.Models;

public class Notification:BaseEntity
{
    public string? UserId { get; private set; }
    public AppUser? User { get; private set; }
    public string? Message { get; private set; }
    public NotificationType Type { get; private set; }
    public NotificationStatus Status { get; private set; }
}
