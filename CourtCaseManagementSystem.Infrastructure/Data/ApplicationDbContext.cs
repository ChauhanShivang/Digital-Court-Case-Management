using Microsoft.EntityFrameworkCore;
using CourtCaseManagementSystem.Core.Entities;

namespace CourtCaseManagementSystem.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Court> Courts => Set<Court>();
    public DbSet<Case> Cases => Set<Case>();
    public DbSet<Hearing> Hearings => Set<Hearing>();
    public DbSet<Document> Documents => Set<Document>();
    public DbSet<JudgeAssignment> JudgeAssignments => Set<JudgeAssignment>();
    public DbSet<Judgment> Judgments => Set<Judgment>();
    public DbSet<CaseEvent> CaseEvents => Set<CaseEvent>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Case>()
            .HasOne(c => c.Lawyer)
            .WithMany()
            .HasForeignKey(c => c.LawyerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Case>()
            .HasOne(c => c.CreatedByClerk)
            .WithMany()
            .HasForeignKey(c => c.CreatedByClerkId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin", CreatedAt = DateTime.UtcNow },
            new Role { Id = 2, Name = "Judge", CreatedAt = DateTime.UtcNow },
            new Role { Id = 3, Name = "Lawyer", CreatedAt = DateTime.UtcNow },
            new Role { Id = 4, Name = "Clerk", CreatedAt = DateTime.UtcNow }
        );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, FullName = "System Admin", Email = "admin@court.com", PasswordHash = "admin123", RoleId = 1, IsActive = true, CreatedAt = DateTime.UtcNow },
            new User { Id = 2, FullName = "Justice A. Shah", Email = "judge@court.com", PasswordHash = "judge123", RoleId = 2, IsActive = true, CreatedAt = DateTime.UtcNow },
            new User { Id = 3, FullName = "Adv. Rohan Mehta", Email = "lawyer@court.com", PasswordHash = "lawyer123", RoleId = 3, IsActive = true, CreatedAt = DateTime.UtcNow },
            new User { Id = 4, FullName = "Court Clerk Priya Patel", Email = "clerk@court.com", PasswordHash = "clerk123", RoleId = 4, IsActive = true, CreatedAt = DateTime.UtcNow }
        );

        modelBuilder.Entity<Court>().HasData(
            new Court { Id = 1, Name = "Surat District Court", Location = "Surat", CourtType = "District", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Court { Id = 2, Name = "Ahmedabad District Court", Location = "Ahmedabad", CourtType = "District", IsActive = true, CreatedAt = DateTime.UtcNow },
            new Court { Id = 3, Name = "Gujarat High Court", Location = "Ahmedabad", CourtType = "High Court", IsActive = true, CreatedAt = DateTime.UtcNow }
        );
    }
}