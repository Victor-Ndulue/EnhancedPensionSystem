using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Application.Helpers.DTOs.Requests;

public record NotificationParams(string UserId, string Message, NotificationType Type);
