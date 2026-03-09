using Microsoft.AspNetCore.Mvc;
using CourtCaseManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace CourtCaseManagementSystem.Web.Controllers;

public class DashboardController : BaseController
{
    private readonly ApplicationDbContext _context;
    
    public DashboardController(ApplicationDbContext context)
    {
        _context = context;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        var role = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetInt32("UserId");

        if (string.IsNullOrEmpty(role))
            return RedirectToAction("Login", "Auth");

        if (role == "Judge")
        {
            var assignedCases = await _context.JudgeAssignments
                .Include(j => j.Case)
                .ThenInclude(c => c.Court)
                .Where(j => j.JudgeId == userId)
                .Select(j => j.Case)
                .ToListAsync();

            ViewBag.AssignedCases = assignedCases;
        }

        return View();
    }
}