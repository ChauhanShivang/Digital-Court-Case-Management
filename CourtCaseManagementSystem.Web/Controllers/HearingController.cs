using CourtCaseManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CourtCaseManagementSystem.Core.Entities;

public class HearingController : Controller
{
    private readonly ApplicationDbContext _context;

    public HearingController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var hearings = await _context.Hearings
            .Include(h => h.Case)
            .ThenInclude(c => c.Court)
            .Include(h => h.Case)
            .ThenInclude(c => c.Lawyer)
            .Where(h => h.Case.JudgeAssignments
                .Any(j => j.JudgeId == userId))
            .ToListAsync();

        return View(hearings);
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var hearing = await _context.Hearings
            .Include(h => h.Case)
            .ThenInclude(c => c.Court)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hearing == null)
            return NotFound();

        return View(hearing);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(Hearing hearing)
    {
        var existing = await _context.Hearings.FindAsync(hearing.Id);

        if (existing == null)
            return NotFound();

        existing.Status = hearing.Status;
        existing.Remarks = hearing.Remarks;
        
        _context.CaseEvents.Add(new CaseEvent
        {
            CaseId = existing.CaseId,
            EventType = "Hearing Updated",
            Description = $"Hearing marked as {existing.Status}. Remarks: {existing.Remarks}"
        });
        
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}