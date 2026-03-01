using Microsoft.AspNetCore.Mvc;

namespace CourtCaseManagementSystem.Web.Controllers;

public class RoleController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Switch(string role)
    {
        HttpContext.Session.SetString("UserRole", role);
        return RedirectToAction("Index", "Dashboard");
    }
}