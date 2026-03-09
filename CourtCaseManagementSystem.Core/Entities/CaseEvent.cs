namespace CourtCaseManagementSystem.Core.Entities;

public class CaseEvent
{
    public int Id { get; set; }

    public int CaseId { get; set; }
    public Case? Case { get; set; }

    public string EventType { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime EventDate { get; set; } = DateTime.UtcNow;
}