using Microsoft.AspNetCore.Mvc;

namespace CourtCaseManagementSystem.Web.Controllers;

public class UserController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Create()
    {
        return View();
    }
}