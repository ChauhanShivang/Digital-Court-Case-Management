using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourtCaseManagementSystem.Infrastructure.Data;

namespace CourtCaseManagementSystem.Web.Controllers;

public class LawyerController : BaseController
{
    private readonly ApplicationDbContext _context;

    public LawyerController(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> UpcomingHearings()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var today = DateTime.Today;

        var hearings = await _context.Hearings
            .Include(h => h.Case)
            .ThenInclude(c => c.Court)
            .Where(h => h.Case.LawyerId == userId 
                        && h.HearingDate >= today)
            .OrderBy(h => h.HearingDate)
            .ToListAsync();

        return View(hearings);
    }
    
    public async Task<IActionResult> CaseHistory()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var events = await _context.CaseEvents
            .Include(e => e.Case)
            .Where(e => e.Case.LawyerId == userId)
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();

        return View(events);
    }
}