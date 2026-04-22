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

    public async Task<IActionResult> AssignedCases(string? search, string? sort)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var query = _context.JudgeAssignments
            .Include(j => j.Case)
            .ThenInclude(c => c.Court)
            .Where(j => j.JudgeId == userId)
            .Select(j => j.Case)
            .AsQueryable();

        // 🔍 SEARCH
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c =>
                c.CaseNumber.Contains(search) ||
                c.Title.Contains(search));
        }

        // ↕ SORT
        if (string.IsNullOrEmpty(sort))
            sort = "latest";

        if (sort == "latest")
            query = query.OrderByDescending(c => c.CreatedAt);

        else if (sort == "oldest")
            query = query.OrderBy(c => c.CreatedAt);

        else if (sort == "priority")
            query = query.OrderByDescending(c => c.PriorityScore);

        var cases = await query.ToListAsync();

        ViewBag.Search = search;
        ViewBag.Sort = sort;

        return View(cases);
    }

    public async Task<IActionResult> JudgmentDrafts()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var cases = await _context.JudgeAssignments
            .Include(j => j.Case)
            .ThenInclude(c => c.Judgment)
            .Where(j => j.JudgeId == userId)
            .Select(j => j.Case)
            .ToListAsync();

        return View(cases);
    }

    public async Task<IActionResult> CaseTimeline(string? search)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var query = _context.CaseEvents
            .Include(e => e.Case)
            .Where(e => e.Case.JudgeAssignments
                .Any(j => j.JudgeId == userId))
            .AsQueryable();

        // 🔍 SEARCH
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(e =>
                e.Case.CaseNumber.Contains(search) ||
                e.EventType.Contains(search));
        }

        var events = await query
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();

        ViewBag.Search = search;

        return View(events);
    }
}