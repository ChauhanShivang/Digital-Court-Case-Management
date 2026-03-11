using CourtCaseManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourtCaseManagementSystem.Web.Controllers;

public class ReportController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.TotalCases = await _context.Cases.CountAsync();
        ViewBag.ActiveCases = await _context.Cases
            .CountAsync(c => c.Status == "Active");

        ViewBag.ClosedCases = await _context.Cases
            .CountAsync(c => c.Status == "Closed");

        return View();
    }
}