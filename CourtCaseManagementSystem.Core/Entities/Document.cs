namespace CourtCaseManagementSystem.Core.Entities;

public class Document
{
    public int Id { get; set; }

    public int CaseId { get; set; }
    public Case? Case { get; set; }

    public int UploadedByUserId { get; set; }
    public User? UploadedBy { get; set; }

    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;
    public string DocumentType { get; set; } = null!;

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}