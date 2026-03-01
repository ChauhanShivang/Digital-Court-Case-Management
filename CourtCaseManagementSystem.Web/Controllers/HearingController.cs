using Microsoft.AspNetCore.Mvc;

namespace CourtCaseManagementSystem.Web.Controllers;

public class HearingController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Edit()
    {
        return View();
    }
    
    public IActionResult Upcoming()
    {
        return View();
    }
}