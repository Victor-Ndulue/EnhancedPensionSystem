using EnhancedPensionSystem_Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace EnhancedPensionSystem_Domain.Models;

public class AppUser : IdentityUser, IBaseEntity
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool IsDeleted { get; set; } = false;
    public AppUserType AppUserType { get; set; }
}
