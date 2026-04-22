using CourtCaseManagementSystem.Core.Entities;
using CourtCaseManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace CourtCaseManagementSystem.Web.Controllers;

public class ChargeSheetController : Controller
{
    private readonly ApplicationDbContext _context;

    public ChargeSheetController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Create(int caseId)
    {
        ViewBag.CaseId = caseId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(int caseId, string charges, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File required");

        // Create folder if not exists
        var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/chargesheets");

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        // Unique filename
        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadPath, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var chargeSheet = new ChargeSheet
        {
            CaseId = caseId,
            Charges = charges,
            FileName = file.FileName,
            FilePath = "/chargesheets/" + fileName,
            UploadedAt = DateTime.UtcNow
        };

        _context.ChargeSheets.Add(chargeSheet);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Case", new { id = caseId });
    }
}