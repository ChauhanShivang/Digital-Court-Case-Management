using CourtCaseManagementSystem.Core.Entities;
using CourtCaseManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace CourtCaseManagementSystem.Web.Controllers;

public class ApplicationController : Controller
{
    private readonly ApplicationDbContext _context;

    public ApplicationController(ApplicationDbContext context)
    {
        _context = context;
    }

    // CREATE
    public IActionResult Create(int caseId)
    {
        return View(new Application { CaseId = caseId });
    }

    [HttpPost]
    [HttpPost]
    public async Task<IActionResult> Create(Application app)
    {
        if (app.CaseId == 0)
        {
            return Content("CaseId is missing ❌");
        }

        _context.Applications.Add(app);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Case", new { id = app.CaseId });
    }

    // UPDATE (JUDGE)
    public async Task<IActionResult> Edit(int id)
    {
        var app = await _context.Applications.FindAsync(id);
        return View(app);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Application model)
    {
        var app = await _context.Applications.FindAsync(model.Id);

        if (app == null) return NotFound();

        app.Status = model.Status;
        app.Remarks = model.Remarks;

        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Case", new { id = app.CaseId });
    }
}