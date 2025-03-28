using EnhancedPensionSystem_Application.Helpers.DTOs.Requests;
using EnhancedPensionSystem_Application.Helpers.DTOs.Responses;
using EnhancedPensionSystem_Application.Helpers.ObjectFormatter;
using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Domain.Enums;
using EnhancedPensionSystem_Domain.Models;
using EnhancedPensionSystem_Infrastructure.Repository.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EnhancedPensionSystem_Application.Services.Implementations;

public sealed class NotificationService : INotificationService
{
    private readonly IGenericRepository<Notification> _notificationRepository;
    public NotificationService(IGenericRepository<Notification> notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task 
        CreateNotificationAsync(NotificationParams notificationParams)
    {
        var notification = new Notification
        {
            UserId = notificationParams.UserId,
            Message = notificationParams.Message,
            Type = notificationParams.Type,
            Status = NotificationStatus.Pending
        };

        await _notificationRepository.AddAsync(notification);
        //Since sharing same db, calling service saves
    }

    public async Task MarkNotificationsAsSent(string[] notificationIds)
    {
        var notifications = _notificationRepository.GetNonDeletedByCondition
        (n => notificationIds.Contains(n.Id) && n.Status != NotificationStatus.Sent);

        if (notifications is null)
            return;
        foreach (var notification in notifications) { notification.Status = NotificationStatus.Sent; }
        await _notificationRepository.SaveChangesAsync();
    }

    public async Task<StandardResponse<IEnumerable<NotificationResponse>>> 
        GetUserNotificationsAsync(string userId)
    {
        var notifications = await _notificationRepository.GetNonDeletedByCondition(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Select(n => new NotificationResponse(n.Id, n.UserId!, n.Message!, n.Type, n.Status, n.CreatedAt))
            .AsNoTracking()
            .ToListAsync();

        return StandardResponse<IEnumerable<NotificationResponse>>.Success(notifications);
    }
}
