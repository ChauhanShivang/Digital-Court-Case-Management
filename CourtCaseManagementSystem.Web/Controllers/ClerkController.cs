using CourtCaseManagementSystem.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourtCaseManagementSystem.Infrastructure.Data;

namespace CourtCaseManagementSystem.Web.Controllers;

public class ClerkController : BaseController
{
    private readonly ApplicationDbContext _context;

    public ClerkController(ApplicationDbContext context)
    {
        _context = context;
    }
    // CASE REGISTRY
    public async Task<IActionResult> CaseRegistry(string? search, string? filter, string? sort)
    {
        var query = _context.Cases
            .Include(c => c.Court)
            .Include(c => c.Lawyer)
            .AsQueryable();

        // 🔍 SEARCH
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(c =>
                c.CaseNumber.Contains(search) ||
                c.Title.Contains(search));
        }

        // 🎯 FILTER
        if (filter == "closed")
            query = query.Where(c => c.Status == "Closed");

        else if (filter == "pending")
            query = query.Where(c => c.Status == "Active" || c.Status == "Registered" || c.Status == "Scheduled");

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
        ViewBag.Filter = filter;
        ViewBag.Sort = sort;

        return View(cases);
    }

    // DAILY CAUSE LIST
    public async Task<IActionResult> DailyCauseList()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);

        var hearings = await _context.Hearings
            .Include(h => h.Case)
            .ThenInclude(c => c.Court)
            .Include(h => h.Case)
            .ThenInclude(c => c.Lawyer)
            .Where(h => h.HearingDate >= today && h.HearingDate < tomorrow)
            .OrderBy(h => h.HearingDate)
            .ToListAsync();

        return View(hearings);
    }
    
    public IActionResult AssignLawyers(int caseId)
    {
        ViewBag.CaseId = caseId;
        ViewBag.Lawyers = _context.Users
            .Where(u => u.Role.Name == "Lawyer")
            .ToList();

        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> AssignLawyers(int caseId, List<int> lawyerIds)
    {
        foreach (var id in lawyerIds)
        {
            var exists = await _context.CaseLawyers
                .AnyAsync(x => x.CaseId == caseId && x.LawyerId == id);

            if (!exists)
            {
                _context.CaseLawyers.Add(new CaseLawyer
                {
                    CaseId = caseId,
                    LawyerId = id
                });
            }
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Case", new { id = caseId });
    }
}