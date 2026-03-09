using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourtCaseManagementSystem.Infrastructure.Data;
using CourtCaseManagementSystem.Core.Entities;

namespace CourtCaseManagementSystem.Web.Controllers;

public class AssignmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public AssignmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET
    public async Task<IActionResult> Index()
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (role != "Clerk")
            return RedirectToAction("Index", "Dashboard");

        ViewBag.Cases = await _context.Cases
            .Where(c => c.Status == "Registered")
            .ToListAsync();

        ViewBag.Judges = await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Role.Name == "Judge")
            .ToListAsync();

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Assign(int caseId, int judgeId, DateTime firstHearingDate)
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (role != "Clerk")
            return RedirectToAction("Index", "Dashboard");

        // Create Judge Assignment
        var assignment = new JudgeAssignment
        {
            CaseId = caseId,
            JudgeId = judgeId,
            AssignedDate = DateTime.UtcNow
        };

        _context.JudgeAssignments.Add(assignment);

        // Create First Hearing
        var hearing = new Hearing
        {
            CaseId = caseId,
            HearingDate = firstHearingDate,
            Status = "Scheduled"
        };

        _context.Hearings.Add(hearing);

        // Update Case Status
        var caseData = await _context.Cases.FindAsync(caseId);
        if (caseData != null)
            caseData.Status = "Scheduled";

        await _context.SaveChangesAsync();
        
        _context.CaseEvents.Add(new CaseEvent
        {
            CaseId = caseId,
            EventType = "Judge Assigned",
            Description = "Judge assigned to case and first hearing scheduled."
        });

        return RedirectToAction("Index");
    }
}