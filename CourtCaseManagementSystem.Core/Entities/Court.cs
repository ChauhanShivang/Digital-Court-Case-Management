namespace CourtCaseManagementSystem.Core.Entities;

public class Court
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string CourtType { get; set; } = null!; // District / High
    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Case>? Cases { get; set; }
}