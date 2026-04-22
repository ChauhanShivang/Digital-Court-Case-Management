using CourtCaseManagementSystem.Core.Entities;
using CourtCaseManagementSystem.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CourtCaseManagementSystem.Infrastructure.Data;
using CourtCaseManagementSystem.Web.Models.DTOs;

namespace CourtCaseManagementSystem.Web.Controllers;

[Route("Case")]
public class CaseController : BaseController
{
    private readonly ApplicationDbContext _context;
    private readonly CasePriorityService _priorityService;

    public CaseController(ApplicationDbContext context, CasePriorityService priorityService)
    {
        _context = context;
        _priorityService = priorityService;
    }

    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        var role = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetInt32("UserId");

        var query = _context.Cases
            .Include(c => c.Court)
            .Include(c => c.Lawyer)
            .AsQueryable();

        if (role == "Lawyer")
        {
            query = query
                .Include(c => c.CaseLawyers)
                .ThenInclude(cl => cl.Lawyer)
                .Where(c =>
                        c.LawyerId == userId // old cases
                        ||
                        c.CaseLawyers.Any(cl => cl.LawyerId == userId) // new cases
                );
        }

        if (role == "Clerk" || role == "Admin")
            query = query;

        if (role == "Judge")
            query = query
                .Include(c => c.Hearings);

        var cases = await query
            .OrderByDescending(c => c.PriorityScore)
            .ToListAsync();

        return View(cases);
    }
    
    [HttpGet("Create")]
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
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create(Case model, int PetitionerLawyerId, int RespondentLawyerId)
{
    var role = HttpContext.Session.GetString("UserRole");
    var clerkId = HttpContext.Session.GetInt32("UserId");

    if (role != "Clerk")
        return RedirectToAction("Index");

    model.CreatedByClerkId = clerkId ?? 0;
    model.FiledDate = DateTime.UtcNow;
    model.CreatedAt = DateTime.UtcNow;

    model.PriorityScore = _priorityService.CalculatePriority(model);

    model.LawyerId = PetitionerLawyerId;

    _context.Cases.Add(model);
    await _context.SaveChangesAsync();

    _context.CaseLawyers.Add(new CaseLawyer
    {
        CaseId = model.Id,
        LawyerId = PetitionerLawyerId
    });

    _context.CaseLawyers.Add(new CaseLawyer
    {
        CaseId = model.Id,
        LawyerId = RespondentLawyerId
    });

    // AUTO JUDGE ASSIGNMENT

    var judges = await _context.Users
        .Include(u => u.Role)
        .Where(u => u.Role.Name == "Judge")
        .ToListAsync();

    var judgeLoad = await _context.JudgeAssignments
        .GroupBy(j => j.JudgeId)
        .Select(g => new
        {
            JudgeId = g.Key,
            Count = g.Count()
        })
        .ToListAsync();

    var selectedJudge = judges
        .OrderBy(j =>
            judgeLoad.FirstOrDefault(x => x.JudgeId == j.Id)?.Count ?? 0
        )
        .FirstOrDefault();

    if (selectedJudge != null)
    {
        _context.JudgeAssignments.Add(new JudgeAssignment
        {
            CaseId = model.Id,
            JudgeId = selectedJudge.Id,
            AssignedDate = DateTime.UtcNow
        });

        model.Status = "Registered";
    }

    // ===============================
    // ✅ EVENTS
    // ===============================

    _context.CaseEvents.Add(new CaseEvent
    {
        CaseId = model.Id,
        EventType = "Case Filed",
        Description = "Case registered with multiple lawyers."
    });

    _context.CaseEvents.Add(new CaseEvent
    {
        CaseId = model.Id,
        EventType = "Judge Auto Assigned",
        Description = $"Judge {selectedJudge?.FullName} assigned automatically."
    });

    await _context.SaveChangesAsync();

    return RedirectToAction("Index");
}
    
    [HttpGet("Assign/{id}")]
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
    
    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var caseData = await _context.Cases
            .Include(c => c.Court)
            .Include(c => c.Lawyer)
            .Include(c => c.Documents)
            .Include(c => c.Hearings)
            .Include(c => c.CaseEvents)
            .Include(c => c.Judgment)
            .Include(c => c.Applications)
            .Include(c => c.FIR)
            .Include(c => c.ChargeSheet)
            .Include(c => c.CaseLawyers)
            .ThenInclude(cl => cl.Lawyer)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (caseData == null)
            return NotFound();

        return View(caseData);
    }
    
    [HttpGet("List")]
    public async Task<IActionResult> List(string? filter, string? sort)
    {
        var query = _context.Cases
            .Include(c => c.Court)
            .Include(c => c.Lawyer)
            .AsQueryable();

        // FILTER
        if (filter == "closed")
            query = query.Where(c => c.Status == "Closed");

        if (filter == "pending")
            query = query.Where(c => c.Status == "Active" || c.Status == "Registered" || c.Status == "Scheduled");

        // DEFAULT SORT
        if (string.IsNullOrEmpty(sort))
            sort = "latest";

        // SORT
        if (sort == "latest")
            query = query.OrderByDescending(c => c.CreatedAt);

        else if (sort == "oldest")
            query = query.OrderBy(c => c.CreatedAt);

        else if (sort == "priority")
            query = query.OrderByDescending(c => c.PriorityScore);

        var cases = await query.ToListAsync();

        ViewBag.Filter = filter;
        ViewBag.Sort = sort;

        return View(cases);
    }
    
    [HttpGet("getcases")]
    public IActionResult GetCases([FromQuery] CaseQueryDto query)
    {
        var cases = _context.Cases.AsQueryable();

        // Filtering
        if (!string.IsNullOrEmpty(query.Status))
            cases = cases.Where(c => c.Status == query.Status);

        if (query.CourtId.HasValue)
            cases = cases.Where(c => c.CourtId == query.CourtId);

        // Pagination
        var result = cases
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        return Ok(result);
    }
    
    public async Task<IActionResult> FileAppeal(int id)
    {
        var caseData = await _context.Cases.FindAsync(id);

        if (caseData == null)
            return NotFound();

        var newCase = new Case
        {
            CaseNumber = "APL-" + DateTime.UtcNow.Ticks,
            Title = caseData.Title + " (Appeal)",
            CaseType = caseData.CaseType,
            CourtId = caseData.CourtId,
            LawyerId = caseData.LawyerId,
            CreatedByClerkId = caseData.CreatedByClerkId,
            Status = "Registered",
            FiledDate = DateTime.UtcNow
        };

        _context.Cases.Add(newCase);

        _context.CaseEvents.Add(new CaseEvent
        {
            CaseId = newCase.Id,
            EventType = "Appeal Filed",
            Description = $"Appeal filed for Case {caseData.CaseNumber}"
        });

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Case", new { id = newCase.Id });
    }
    
    [HttpGet("Allocation")]
    public async Task<IActionResult> Allocation()
    {
        var cases = await _context.Cases
            .Include(c => c.Court)
            .Include(c => c.JudgeAssignments)
            .ThenInclude(j => j.Judge)
            .ToListAsync();

        return View(cases);
    }
}