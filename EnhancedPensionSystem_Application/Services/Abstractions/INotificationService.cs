using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;

namespace EnhancedPensionSystem_Application.Services.Abstractions;

public interface INotificationService
{
    Task CreateNotificationAsync(NotificationParams notificationParams);
    Task MarkNotificationsAsSent(string[] notificationIds);
    Task<StandardResponse<IEnumerable<NotificationResponse>>> GetUserNotificationsAsync(string userId);
}
