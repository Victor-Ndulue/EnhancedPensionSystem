using EnhancedPensionSystem_Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnhancedPensionSystem_Infrastructure.DataContext;

public sealed class AppDbContext:IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions): base(dbContextOptions)
    {        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Member>().HasKey(x => x.Id);
        modelBuilder.Entity<Employer>().HasKey(x => x.Id);
        modelBuilder.Entity<Contribution>().HasKey(x => x.Id);
        modelBuilder.Entity<BenefitEligibility>().HasKey(x => x.Id);
        modelBuilder.Entity<Transaction>().HasKey(x => x.Id);
        modelBuilder.Entity<Notification>().HasKey(x => x.Id);

        modelBuilder.Entity<Member>()
            .HasOne<Employer>(m => m.Employer)
            .WithMany(e => e.Members)
            .HasForeignKey(m => m.EmployerId);

        modelBuilder.Entity<Contribution>()
            .HasOne(c => c.Member)
            .WithMany(m => m.Contributions)
            .HasForeignKey(c => c.MemberId);

        modelBuilder.Entity<BenefitEligibility>()
            .HasOne(b => b.Member)
            .WithOne()
            .HasForeignKey<BenefitEligibility>(b => b.MemberId);

        modelBuilder.Entity<Transaction>()
            .HasOne(b => b.Contribution)
            .WithOne()
            .HasForeignKey<Transaction>(b => b.ContributionId);

        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u=>u.Notifications)
            .HasForeignKey(b => b.UserId);

        foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(decimal)))
        {
            property.SetColumnType("decimal(9,2)");
        }
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Employer> Employers { get; set; }
    public DbSet<Contribution> Contributions { get; set; }
    public DbSet<BenefitEligibility> BenefitEligibilities { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Notification> Notifications { get; set; }
}
