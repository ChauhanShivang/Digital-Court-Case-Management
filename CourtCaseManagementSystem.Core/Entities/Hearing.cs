namespace CourtCaseManagementSystem.Core.Entities;

public class Hearing
{
    public int Id { get; set; }

    public int CaseId { get; set; }
    public Case? Case { get; set; }

    public DateTime HearingDate { get; set; }
    public string Status { get; set; } = "Scheduled";

    public string? Remarks { get; set; }
    public int AdjournmentCount { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}