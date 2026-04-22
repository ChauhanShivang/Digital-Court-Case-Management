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
    public async Task<IActionResult> Create(User user, string password)
    {
        user.PasswordHash = password; // simple for project
        user.CreatedAt = DateTime.UtcNow;
        user.IsActive = true;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        ViewBag.Roles = await _context.Roles.ToListAsync();

        return View(user);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);

        if (existingUser == null)
            return NotFound();

        existingUser.FullName = user.FullName;
        existingUser.Email = user.Email;
        existingUser.RoleId = user.RoleId;
        existingUser.IsActive = user.IsActive;

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null)
            return NotFound();

        return View(user);
    }
    
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
    
    
}