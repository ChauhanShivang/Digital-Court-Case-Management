using CourtCaseManagementSystem.Core.Entities;
using CourtCaseManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourtCaseManagementSystem.Web.Controllers;

public class CourtController : BaseController
{
    private readonly ApplicationDbContext _context;

    public CourtController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var courts = await _context.Courts.ToListAsync();
        return View(courts);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Court model)
    {
        model.CreatedAt = DateTime.UtcNow;
        model.IsActive = true;

        _context.Courts.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}