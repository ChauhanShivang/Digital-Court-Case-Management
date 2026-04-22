namespace CourtCaseManagementSystem.Core.Entities;

public class AdjournmentRequest
{
    public int Id { get; set; }

    public int HearingId { get; set; }
    public Hearing? Hearing { get; set; }

    public int RequestedByUserId { get; set; }
    public User? RequestedBy { get; set; }

    public string Reason { get; set; } = null!;

    public string Status { get; set; } = "Pending"; // Pending / Approved / Rejected

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
}