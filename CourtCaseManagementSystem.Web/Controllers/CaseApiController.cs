using Microsoft.AspNetCore.Mvc;
using CourtCaseManagementSystem.Infrastructure.Data;
using CourtCaseManagementSystem.Web.Models.DTOs;

namespace CourtCaseManagementSystem.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CaseApiController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public CaseApiController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetCases([FromQuery] CaseQueryDto query)
    {
        var cases = _context.Cases.AsQueryable();

        if (!string.IsNullOrEmpty(query.Status))
            cases = cases.Where(c => c.Status == query.Status);

        if (query.CourtId.HasValue)
            cases = cases.Where(c => c.CourtId == query.CourtId);

        var result = cases
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToList();

        return Ok(result);
    }
    
    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok("API working");
    }
}