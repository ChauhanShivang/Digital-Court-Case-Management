namespace CourtCaseManagementSystem.Core.Entities;

public class Case
{
    public int Id { get; set; }
    public string CaseNumber { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string CaseType { get; set; } = null!; // Civil / Criminal
    public string? Description { get; set; }

    public DateTime FiledDate { get; set; }
    public string Status { get; set; } = "Registered";

    public int PriorityScore { get; set; } = 0;

    public int CourtId { get; set; }
    public Court? Court { get; set; }

    public int LawyerId { get; set; }
    public User? Lawyer { get; set; }

    public int CreatedByClerkId { get; set; }
    public User? CreatedByClerk { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Hearing>? Hearings { get; set; }
    public ICollection<Document>? Documents { get; set; }
    public ICollection<CaseEvent>? CaseEvents { get; set; }
    public ICollection<JudgeAssignment>? JudgeAssignments { get; set; }
    
    public Judgment? Judgment { get; set; }
}