namespace CourtCaseManagementSystem.Web.Models.DTOs;

public class CaseQueryDto
{
    public string? Status { get; set; }
    public int? CourtId { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}