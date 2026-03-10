using CourtCaseManagementSystem.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourtCaseManagementSystem.Infrastructure.Data;

namespace CourtCaseManagementSystem.Web.Controllers;

public class CaseController : BaseController
{
    private readonly ApplicationDbContext _context;

    public CaseController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var role = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetInt32("UserId");

        var query = _context.Cases
            .Include(c => c.Court)
            .Include(c => c.Lawyer)
            .AsQueryable();

        if (role == "Lawyer")
            query = query.Where(c => c.LawyerId == userId);

        if (role == "Clerk" || role == "Admin")
            query = query;

        if (role == "Judge")
            query = query
                .Include(c => c.Hearings);

        var cases = await query.ToListAsync();

        return View(cases);
    }
    
    public async Task<IActionResult> Create()
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (role != "Clerk")
            return RedirectToAction("Index");

        ViewBag.Courts = await _context.Courts.ToListAsync();
        ViewBag.Lawyers = await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Role.Name == "Lawyer")
            .ToListAsync();

        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Case model)
    {
        var role = HttpContext.Session.GetString("UserRole");
        var clerkId = HttpContext.Session.GetInt32("UserId");

        if (role != "Clerk")
            return RedirectToAction("Index");

        model.CreatedByClerkId = clerkId ?? 0;
        model.FiledDate = DateTime.UtcNow;
        model.CreatedAt = DateTime.UtcNow;

        _context.Cases.Add(model);
        await _context.SaveChangesAsync();
        
        _context.CaseEvents.Add(new CaseEvent
        {
            CaseId = model.Id,
            EventType = "Case Filed",
            Description = "Case registered by court clerk."
        });
        
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Assign(int id)
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (role != "Clerk")
            return RedirectToAction("Index");

        var caseData = await _context.Cases.FindAsync(id);

        ViewBag.Case = caseData;

        ViewBag.Judges = await _context.Users
            .Include(u => u.Role)
            .Where(u => u.Role.Name == "Judge")
            .ToListAsync();

        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Assign(int caseId, int judgeId)
    {
        var role = HttpContext.Session.GetString("UserRole");
        if (role != "Clerk")
            return RedirectToAction("Index");

        var assignment = new JudgeAssignment
        {
            CaseId = caseId,
            JudgeId = judgeId,
            AssignedDate = DateTime.UtcNow
        };

        var caseData = await _context.Cases.FindAsync(caseId);
        if (caseData != null)
            caseData.Status = "Registered";

        _context.JudgeAssignments.Add(assignment);
        
        _context.CaseEvents.Add(new CaseEvent
        {
            CaseId = caseId,
            EventType = "Judge Assigned",
            Description = "Judge assigned to the case."
        });
        
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Details(int id)
    {
        var caseData = await _context.Cases
            .Include(c => c.Court)
            .Include(c => c.Lawyer)
            .Include(c => c.Documents)
            .Include(c => c.Hearings)
            .Include(c => c.CaseEvents)
            .Include(c => c.Judgment)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (caseData == null)
            return NotFound();

        return View(caseData);
    }
}