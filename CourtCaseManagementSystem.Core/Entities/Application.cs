namespace CourtCaseManagementSystem.Core.Entities;

public class Application
{
    public int Id { get; set; }

    public int CaseId { get; set; }
    public Case? Case { get; set; }

    public string Type { get; set; } = null!; // Bail, Stay, etc.

    public string Status { get; set; } = "Filed";

    public string? Remarks { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}