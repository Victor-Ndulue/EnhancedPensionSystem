using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Domain.Models;

public class Notification:BaseEntity
{
    public string? UserId { get;  set; }
    public AppUser? User { get;  set; }
    public string? Message { get;  set; }
    public NotificationType Type { get;  set; }
    public NotificationStatus Status { get;  set; }
}
