namespace CourtCaseManagementSystem.Core.Entities;

public class JudgeAssignment
{
    public int Id { get; set; }

    public int CaseId { get; set; }
    public Case? Case { get; set; }

    public int JudgeId { get; set; }
    public User? Judge { get; set; }

    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
}