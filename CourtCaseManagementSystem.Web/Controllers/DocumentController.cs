using Microsoft.AspNetCore.Mvc;

namespace CourtCaseManagementSystem.Web.Controllers;

public class DocumentController : Controller
{
    // GET
    public IActionResult Upload()
    {
        return View();
    }
    
    public IActionResult Library()
    {
        return View();
    }
}