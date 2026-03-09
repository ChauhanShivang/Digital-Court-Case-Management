namespace CourtCaseManagementSystem.Core.Entities;

public class Judgment
{
    public int Id { get; set; }

    public int CaseId { get; set; }
    public Case? Case { get; set; }

    public int JudgeId { get; set; }
    public User? Judge { get; set; }

    public string Content { get; set; } = null!;
    public string Status { get; set; } = "Draft";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinalizedAt { get; set; }
}