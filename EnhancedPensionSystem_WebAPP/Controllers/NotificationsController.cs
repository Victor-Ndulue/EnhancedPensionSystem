using EnhancedPensionSystem_Application.Services.Abstractions;
using EnhancedPensionSystem_Application.UnitOfWork.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace EnhancedPensionSystem_WebAPP.Controllers;

/// <summary>
/// Controller for managing user notifications.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : BaseController
{
    private readonly INotificationService _notificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationsController"/> class.
    /// </summary>
    /// <param name="serviceManager">The service manager that provides access to notification-related services.</param>
    public NotificationsController(IServiceManager serviceManager)
    {
        _notificationService = serviceManager.NotificationService;
    }

    /// <summary>
    /// Retrieves all notifications for a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of notifications for the specified user.</returns>
    /// <response code="200">Notifications retrieved successfully.</response>
    /// <response code="404">User not found or no notifications available.</response>
    [HttpGet("user-notifications")]
    public async Task<IActionResult> GetUserNotificationsAsync([FromQuery] string userId)
    {
        var result = await _notificationService.GetUserNotificationsAsync(userId);
        return StatusCode(result.StatusCode, result);
    }
}
