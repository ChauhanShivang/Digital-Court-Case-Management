namespace CourtCaseManagementSystem.Core.Entities;

public class FIR
{
    public int Id { get; set; }

    public string FIRNumber { get; set; } = null!;
    public string PoliceStation { get; set; } = null!;
    public string Description { get; set; } = null!;

    public DateTime FiledDate { get; set; } = DateTime.UtcNow;

    public int CaseId { get; set; }
    public Case? Case { get; set; }
}