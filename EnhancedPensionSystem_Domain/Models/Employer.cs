namespace EnhancedPensionSystem_Domain.Models;

public class Employer: AppUser
{
    public string? CompanyName { get; set; }
    public string? RegistrationNumber { get; set; }
    public bool? IsActive { get; set; }
    public virtual ICollection<Member>? Members { get; set; }
}
