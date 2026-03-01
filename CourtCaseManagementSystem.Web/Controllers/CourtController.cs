using Microsoft.AspNetCore.Mvc;

namespace CourtCaseManagementSystem.Web.Controllers;

public class CourtController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}