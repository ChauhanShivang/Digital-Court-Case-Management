using CourtCaseManagementSystem.Core.Entities;
using CourtCaseManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourtCaseManagementSystem.Web.Controllers;

public class AdjournmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdjournmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // 🟢 Lawyer → Request
    public IActionResult Request(int hearingId)
    {
        ViewBag.HearingId = hearingId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Request(int hearingId, string reason)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var request = new AdjournmentRequest
        {
            HearingId = hearingId,
            RequestedByUserId = userId ?? 0,
            Reason = reason
        };

        _context.AdjournmentRequests.Add(request);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Hearing");
    }

    // 🟡 Judge/Clerk → View Requests
    public async Task<IActionResult> Index()
    {
        var requests = await _context.AdjournmentRequests
            .Include(r => r.Hearing)
            .ThenInclude(h => h.Case)
            .ToListAsync();

        return View(requests);
    }

    // 🔵 Approve
    public async Task<IActionResult> Approve(int id)
    {
        var req = await _context.AdjournmentRequests
            .Include(r => r.Hearing)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (req == null) return NotFound();

        req.Status = "Approved";

        req.Hearing!.Status = "Adjourned";
        req.Hearing.AdjournmentCount++;

        _context.CaseEvents.Add(new CaseEvent
        {
            CaseId = req.Hearing.CaseId,
            EventType = "Adjournment Approved",
            Description = req.Reason
        });

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    // 🔴 Reject
    public async Task<IActionResult> Reject(int id)
    {
        var req = await _context.AdjournmentRequests.FindAsync(id);

        if (req == null) return NotFound();

        req.Status = "Rejected";

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}