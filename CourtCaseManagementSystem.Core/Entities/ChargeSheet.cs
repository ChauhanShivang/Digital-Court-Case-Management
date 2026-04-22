namespace CourtCaseManagementSystem.Core.Entities;

public class ChargeSheet
{
    public int Id { get; set; }

    public string Charges { get; set; } = null!;
    public DateTime FiledDate { get; set; } = DateTime.UtcNow;

    public int CaseId { get; set; }
    public Case? Case { get; set; }
    public string? FileName { get; set; }
    public string? FilePath { get; set; }
    public DateTime UploadedAt { get; set; }
}