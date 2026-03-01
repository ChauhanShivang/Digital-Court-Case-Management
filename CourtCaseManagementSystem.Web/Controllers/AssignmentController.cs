using Microsoft.AspNetCore.Mvc;

namespace CourtCaseManagementSystem.Web.Controllers;

public class AssignmentController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}