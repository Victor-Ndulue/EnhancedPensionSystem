using EnhancedPensionSystem_Domain.Enums;

namespace EnhancedPensionSystem_Application.Helpers.DTOs.Responses;

public record NotificationResponse(string? Id, string userId, string message, NotificationType type, NotificationStatus status, DateTime createdAt);
