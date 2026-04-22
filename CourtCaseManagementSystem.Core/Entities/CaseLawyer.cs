namespace CourtCaseManagementSystem.Core.Entities;

public class CaseLawyer
{
    public int Id { get; set; }

    public int CaseId { get; set; }
    public Case? Case { get; set; }

    public int LawyerId { get; set; }
    public User? Lawyer { get; set; }
}