namespace EnhancedPensionSystem_Domain.Models;

public class Member:AppUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? EmployerId { get; set; }
    public Employer? Employer { get; set; }
    public virtual ICollection<Contribution>? Contributions { get; set; }
}
