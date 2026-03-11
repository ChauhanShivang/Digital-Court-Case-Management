using CourtCaseManagementSystem.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using CourtCaseManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CourtCaseManagementSystem.Web.Controllers;

public class UserController : BaseController
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _context.Users
            .Include(u => u.Role)
            .ToListAsync();

        return View(users);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Roles = await _context.Roles.ToListAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(User model)
    {
        model.CreatedAt = DateTime.UtcNow;

        _context.Users.Add(model);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}