using Microsoft.AspNetCore.Mvc;

namespace CourtCaseManagementSystem.Web.Controllers;

public class CauseListController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}