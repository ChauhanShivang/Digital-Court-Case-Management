using CourtCaseManagementSystem.Core.Entities;

namespace CourtCaseManagementSystem.Core.Services;

public class CasePriorityService
{
    public int CalculatePriority(Case c)
    {
        int score = 0;

        // Case type
        if (c.CaseType == "Criminal")
            score += 40;
        else
            score += 20;

        // Case age
        var months = (DateTime.UtcNow - c.FiledDate).Days / 30;

        if (months > 12)
            score += 30;
        else if (months > 6)
            score += 20;

        // Hearings count
        if (c.Hearings != null)
            score += c.Hearings.Count * 5;

        return score;
    }
}