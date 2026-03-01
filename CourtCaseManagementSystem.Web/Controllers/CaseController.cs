using Microsoft.AspNetCore.Mvc;

namespace CourtCaseManagementSystem.Web.Controllers;

public class CaseController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Details()
    {
        return View();
    }
    
    public IActionResult Create()
    {
        return View();
    }
    
    public IActionResult History()
    {
        return View();
    }
    
    public IActionResult Registry()
    {
        return View();
    }
}