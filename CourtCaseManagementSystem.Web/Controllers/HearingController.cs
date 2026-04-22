using CourtCaseManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CourtCaseManagementSystem.Core.Entities;
using CourtCaseManagementSystem.Core.Services;

public class HearingController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly EmailService _emailService;

    public HearingController(ApplicationDbContext context, EmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<IActionResult> Index(string? search, string? sort)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var query = _context.Hearings
            .Include(h => h.Case)
            .ThenInclude(c => c.Court)
            .Include(h => h.Case)
            .ThenInclude(c => c.Lawyer)
            .Where(h => h.Case.JudgeAssignments
                .Any(j => j.JudgeId == userId))
            .AsQueryable();

        // 🔍 SEARCH
        if (!string.IsNullOrEmpty(search))
        {
            query = query.Where(h =>
                h.Case.CaseNumber.Contains(search) ||
                h.Case.Title.Contains(search));
        }

        // ↕ SORT
        if (string.IsNullOrEmpty(sort))
            sort = "latest";

        if (sort == "latest")
            query = query.OrderByDescending(h => h.HearingDate);

        else if (sort == "oldest")
            query = query.OrderBy(h => h.HearingDate);

        var hearings = await query.ToListAsync();

        ViewBag.Search = search;
        ViewBag.Sort = sort;

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
    
    public IActionResult Create(int caseId)
    {
        var hearing = new Hearing
        {
            CaseId = caseId
        };

        return View(hearing);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Hearing hearing)
    {
        _context.Hearings.Add(hearing);
        await _context.SaveChangesAsync();
        
        var caseData = await _context.Cases
            .Include(c => c.Lawyer)
            .FirstOrDefaultAsync(c => c.Id == hearing.CaseId);

        if (caseData?.Lawyer?.Email != null)
        {
            _emailService.SendEmail(
                caseData.Lawyer.Email,
                "Hearing Scheduled",
                $"Your case {caseData.CaseNumber} has a hearing on {hearing.HearingDate:dd MMM yyyy}"
            );
        }

        return RedirectToAction("Details", "Case", new { id = hearing.CaseId });
    }
}