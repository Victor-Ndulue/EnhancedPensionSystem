namespace EnhancedPensionSystem_Domain.Models;

public class Employer
{
    public string? CompanyName { get; set; }
    public string? RegistrationNumber { get; set; }
    public virtual ICollection<Member>? Members { get; set; }
}
