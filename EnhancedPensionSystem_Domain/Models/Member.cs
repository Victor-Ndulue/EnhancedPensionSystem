namespace EnhancedPensionSystem_Domain.Models;

public class Member:AppUser
{
    public DateTime DateOfBirth { get; private set; }
    public Guid EmployerId { get; private set; }
    public Employer? Employer { get; private set; }
    public virtual ICollection<Contribution>? Contributions { get; set; }
}
