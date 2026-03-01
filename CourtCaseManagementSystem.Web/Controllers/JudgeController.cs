using Microsoft.AspNetCore.Mvc;

namespace CourtCaseManagementSystem.Web.Controllers;

public class JudgeController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult AssignedCases()
    {
        return View();
    }

    public IActionResult JudgmentDrafts()
    {
        return View();
    }

    public IActionResult CaseTimeline()
    {
        return View();
    }
}