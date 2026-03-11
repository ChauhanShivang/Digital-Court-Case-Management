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
        var judgment = await _context.Judgments.FindAsync(model.Id);

        if (judgment == null)
            return NotFound();

        judgment.Content = model.Content;

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Case", new { id = judgment.CaseId });
    }
}