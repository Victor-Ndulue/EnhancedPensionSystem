using EnhancedPensionSystem_Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace EnhancedPensionSystem_Domain.Models;

public class AppUser : IdentityUser, IBaseEntity
{
    public bool IsDeleted { get; set; } = false;
    public AppUserType AppUserType { get; set; }
    public virtual ICollection<Notification>? Notifications { get; set; }
}
