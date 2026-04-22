using Microsoft.AspNetCore.Mvc;
using CourtCaseManagementSystem.Core.Entities;
using CourtCaseManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CourtCaseManagementSystem.Web.Controllers;

public class JudgmentController : BaseController
{
    private readonly ApplicationDbContext _context;

    public JudgmentController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IActionResult> Index(string? search, string? sort)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var query = _context.Judgments
            .Include(j => j.Case)
            .Where(j => j.JudgeId == userId)
            .AsQueryable();

        // 🔍 SEARCH
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(j =>
                j.Case.CaseNumber.Contains(search) ||
                j.Case.Title.Contains(search));
        }

        // ↕ SORT
        if (string.IsNullOrEmpty(sort))
            sort = "latest";

        if (sort == "latest")
            query = query.OrderByDescending(j => j.CreatedAt);

        else if (sort == "oldest")
            query = query.OrderBy(j => j.CreatedAt);

        var drafts = await query.ToListAsync();

        ViewBag.Search = search;
        ViewBag.Sort = sort;

        return View(drafts);
    }

    public async Task<IActionResult> Create(int caseId)
    {
        var caseData = await _context.Cases
            .Include(c => c.Court)
            .FirstOrDefaultAsync(c => c.Id == caseId);

        ViewBag.Case = caseData;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(int caseId, string content)
    {
        var judgeId = HttpContext.Session.GetInt32("UserId");

        var judgment = new Judgment
        {
            CaseId = caseId,
            JudgeId = judgeId ?? 0,
            Content = content,
            Status = "Draft"
        };

        _context.Judgments.Add(judgment);

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Case", new { id = caseId });
    }

    public async Task<IActionResult> Edit(int caseId)
    {
        var judgment = await _context.Judgments
            .FirstOrDefaultAsync(j => j.CaseId == caseId);

        return View(judgment);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Judgment model)
    {
        var existing = await _context.Judgments
            .Include(j => j.Case)
            .FirstOrDefaultAsync(j => j.Id == model.Id);

        if (existing == null)
            return NotFound();

        existing.Content = model.Content;
        existing.Status = model.Status;

// ✅ WHEN JUDGMENT DELIVERED → CLOSE CASE
        if (model.Status == "Delivered")
        {
            existing.FinalizedAt = DateTime.UtcNow;

            if (existing.Case != null)
            {
                existing.Case.Status = "Closed"; // 🔥 THIS LINE FIXES EVERYTHING
            }

            _context.CaseEvents.Add(new CaseEvent
            {
                CaseId = existing.CaseId,
                EventType = "Case Closed",
                Description = "Case closed after final judgment."
            });
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Case", new { id = existing.CaseId });
    }
    
    public async Task<IActionResult> Finalize(int id)
    {
        var judgment = await _context.Judgments
            .Include(j => j.Case)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (judgment == null)
            return NotFound();

        judgment.Status = "Final";
        judgment.FinalizedAt = DateTime.UtcNow;

        if (judgment.Case != null)
            judgment.Case.Status = "Closed";

        _context.CaseEvents.Add(new CaseEvent
        {
            CaseId = judgment.CaseId,
            EventType = "Judgment Finalized",
            Description = "Final judgment issued by the court."
        });

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Case", new { id = judgment.CaseId });
    }
    
    public async Task<IActionResult> Download(int id)
    {
        var judgment = await _context.Judgments
            .Include(j => j.Case)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (judgment == null)
            return NotFound();

        var content = $"Case: {judgment.Case?.CaseNumber}\n\n" +
                      $"Judgment:\n{judgment.Content}\n\n" +
                      $"Status: {judgment.Status}";

        var bytes = System.Text.Encoding.UTF8.GetBytes(content);

        return File(bytes, "text/plain", "Judgment.txt");
    }
}