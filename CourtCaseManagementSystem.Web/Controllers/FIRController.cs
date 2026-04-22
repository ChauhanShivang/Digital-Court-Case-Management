using CourtCaseManagementSystem.Core.Entities;
using CourtCaseManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace CourtCaseManagementSystem.Web.Controllers;

public class FIRController : Controller
{
    private readonly ApplicationDbContext _context;

    public FIRController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create(int caseId)
    {
        ViewBag.CaseId = caseId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(FIR model)
    {
        _context.FIRs.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Case", new { id = model.CaseId });
    }
}