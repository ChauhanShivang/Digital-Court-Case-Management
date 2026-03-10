using CourtCaseManagementSystem.Infrastructure.Data;
using CourtCaseManagementSystem.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourtCaseManagementSystem.Web.Controllers;

public class DocumentController : Controller
{
    private readonly ApplicationDbContext _context;

    public DocumentController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET
    public IActionResult Upload()
    {
        return View();
    }
    
    public IActionResult Library()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(int caseId, IFormFile file, string documentType)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file selected");

        var allowedExtensions = new[] { ".pdf", ".docx", ".jpg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            return BadRequest("Invalid file type");

        if (file.Length > 5 * 1024 * 1024)
            return BadRequest("File too large");

        var randomName = Guid.NewGuid().ToString() + extension;

        var path = Path.Combine("Storage/Documents", randomName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var userId = HttpContext.Session.GetInt32("UserId");

        var document = new Document
        {
            CaseId = caseId,
            UploadedByUserId = userId ?? 0,
            FileName = file.FileName,
            StoredFileName = randomName,
            FilePath = path,
            DocumentType = documentType
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Case", new { id = caseId });
    }
    
    public async Task<IActionResult> Download(int id)
    {
        var userRole = HttpContext.Session.GetString("UserRole");
        var userId = HttpContext.Session.GetInt32("UserId");

        var document = await _context.Documents
            .Include(d => d.Case)
            .ThenInclude(c => c.JudgeAssignments)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (document == null)
            return NotFound();

        bool authorized = false;

        if (userRole == "Admin" || userRole == "Clerk")
            authorized = true;

        if (userRole == "Lawyer" && document.Case?.LawyerId == userId)
            authorized = true;

        if (userRole == "Judge")
        {
            authorized = document.Case?.JudgeAssignments
                .Any(j => j.JudgeId == userId) ?? false;
        }

        if (!authorized)
            return Forbid();

        var path = Path.Combine("Storage/Documents", document.StoredFileName);

        var bytes = await System.IO.File.ReadAllBytesAsync(path);

        return File(bytes, "application/octet-stream", document.FileName);
    }
}