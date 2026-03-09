using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourtCaseManagementSystem.Infrastructure.Data;

namespace CourtCaseManagementSystem.Web.Controllers;

public class JudgeController : Controller
{
    private readonly ApplicationDbContext _context;

    public JudgeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> AssignedCases()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var cases = await _context.JudgeAssignments
            .Include(j => j.Case)
            .ThenInclude(c => c.Court)
            .Include(j => j.Case)
            .ThenInclude(c => c.Lawyer)
            .Where(j => j.JudgeId == userId)
            .Select(j => j.Case)
            .ToListAsync();

        return View(cases);
    }

    public IActionResult JudgmentDrafts()
    {
        return View();
    }

    public async Task<IActionResult> CaseTimeline(int caseId)
    {
        var events = await _context.CaseEvents
            .Where(e => e.CaseId == caseId)
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();

        return View(events);
    }
}