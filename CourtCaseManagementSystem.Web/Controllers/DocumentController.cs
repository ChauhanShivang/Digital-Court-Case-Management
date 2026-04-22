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
    
    public async Task<IActionResult> Library()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        var documents = await _context.Documents
            .Include(d => d.Case)
            .Where(d => d.Case.LawyerId == userId)
            .OrderByDescending(d => d.UploadedAt)
            .ToListAsync();

        return View(documents);
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

        var folder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Storage",
            "Documents"
        );

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        var path = Path.Combine(folder, randomName);

        using (var stream = new FileStream(path, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        
        var isVakalatnama = documentType == "Vakalatnama";

        var userId = HttpContext.Session.GetInt32("UserId");

        var document = new Document
        {
            CaseId = caseId,
            UploadedByUserId = userId ?? 0,
            FileName = file.FileName,
            StoredFileName = randomName,
            FilePath = path,
            DocumentType = documentType,
            IsVakalatnama = isVakalatnama
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync();
        
        _context.CaseEvents.Add(new CaseEvent
        {
            CaseId = caseId,
            EventType = "Document Uploaded",
            Description = $"Document '{file.FileName}' uploaded."
        });

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
        
        if (userRole == "Lawyer")
        {
            var hasVakalatnama = await _context.Documents
                .AnyAsync(d => d.CaseId == document.CaseId 
                               && d.IsVakalatnama 
                               && d.UploadedByUserId == userId);

            if (!hasVakalatnama)
                return Forbid("Upload Vakalatnama first.");
        }

        if (userRole == "Judge")
        {
            authorized = document.Case?.JudgeAssignments
                .Any(j => j.JudgeId == userId) ?? false;
        }

        if (!authorized)
            return Forbid();

        var path = Path.Combine(
            Directory.GetCurrentDirectory(),
            "Storage",
            "Documents",
            document.StoredFileName
        );

        if (!System.IO.File.Exists(path))
        {
            return NotFound("File not found on server.");
        }

        var bytes = await System.IO.File.ReadAllBytesAsync(path);

        return File(bytes, "application/octet-stream", document.FileName);
    }
}