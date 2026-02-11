using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test_Avtobus1.Models;
using test_Avtobus1.Services;

namespace test_Avtobus1.Controllers;

[Route("")]
public class UrlController : Controller
{
    private readonly AppDbContext _db;

    public UrlController(AppDbContext db)
    {
        _db = db;
    }

    // GET /{shortCode}
    [HttpGet("{shortCode}")]
    public async Task<IActionResult> RedirectToOriginal(string shortCode)
    {
        var entity = await _db.ShortUrls
            .FirstOrDefaultAsync(x => x.ShortCode == shortCode);

        if (entity == null)
            return NotFound();

        entity.ClickCount++;
        await _db.SaveChangesAsync();

        return Redirect(entity.OriginalUrl);
    }
}