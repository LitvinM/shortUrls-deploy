using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using test_Avtobus1;
using test_Avtobus1.Models;
using test_Avtobus1.Services;

namespace test_avtobus1.Tests;

public class PerformanceTests
{
    private AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task Generate_10000_ShortUrls_Should_Be_Fast_Enough()
    {
        var db = CreateDb();
        db.ChangeTracker.AutoDetectChangesEnabled = false;
        var service = new UrlService(db);

        var sw = System.Diagnostics.Stopwatch.StartNew();

        for (int i = 0; i < 10_000; i++)
        {
            await service.CreateAsync($"https://example.com/{i}");
        }

        sw.Stop();

        sw.ElapsedMilliseconds.Should().BeLessThan(10000);
    }
    
    [Fact]
    public async Task Redirect_10000_Times_Should_Be_Handled()
    {
        var db = CreateDb();

        var entity = new ShortUrl
        {
            OriginalUrl = "https://google.com",
            ShortCode = "PERF1",
            CreatedAt = DateTime.UtcNow,
            ClickCount = 0
        };

        db.ShortUrls.Add(entity);
        db.SaveChanges();

        var sw = System.Diagnostics.Stopwatch.StartNew();

        for (int i = 0; i < 10_000; i++)
        {
            var e = await db.ShortUrls.FirstAsync(x => x.ShortCode == "PERF1");
            e.ClickCount++;
            await db.SaveChangesAsync();
        }

        sw.Stop();

        sw.ElapsedMilliseconds.Should().BeLessThan(5000);
    }
}
