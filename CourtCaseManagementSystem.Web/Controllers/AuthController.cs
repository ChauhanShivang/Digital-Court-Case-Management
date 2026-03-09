using Microsoft.AspNetCore.Mvc;
using CourtCaseManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CourtCaseManagementSystem.Web.Controllers;

public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password && u.IsActive);

        if (user == null)
        {
            ViewBag.Error = "Invalid credentials";
            return View();
        }

        HttpContext.Session.SetString("UserRole", user.Role.Name);
        HttpContext.Session.SetInt32("UserId", user.Id);

        return RedirectToAction("Index", "Dashboard");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}