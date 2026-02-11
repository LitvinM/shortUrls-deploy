using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test_Avtobus1.Models;
using test_Avtobus1.Services;

namespace test_Avtobus1.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _db;
    private readonly UrlService _service;
    private readonly UrlValidator _validator;

    public HomeController(AppDbContext db, UrlService service, UrlValidator validator)
    {
        _db = db;
        _service = service;
        _validator = validator;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _db.ShortUrls
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();

        return View(items);
    }

    [HttpPost]
    public async Task<IActionResult> Create(string originalUrl)
    {
        if (!_validator.IsValid(originalUrl))
        {
            TempData["Error"] = 
                "Некорректный URL. Разрешены только http://, https:// или домен вида example.com";
            return RedirectToAction(nameof(Index));
        }

        await _service.CreateAsync(originalUrl.Trim());
        return RedirectToAction(nameof(Index));
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var entity = await _db.ShortUrls.FindAsync(id);
        if (entity == null)
            return NotFound();

        _db.ShortUrls.Remove(entity);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, string originalUrl)
    {
        if (!Uri.TryCreate(originalUrl, UriKind.Absolute, out _))
        {
            return BadRequest("Некорректный URL");
        }

        var entity = await _db.ShortUrls.FindAsync(id);
        if (entity == null)
            return NotFound();

        entity.OriginalUrl = originalUrl;
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}