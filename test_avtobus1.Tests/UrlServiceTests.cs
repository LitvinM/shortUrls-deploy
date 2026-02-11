using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using test_Avtobus1;
using test_Avtobus1.Services;

namespace test_avtobus1.Tests;

public class UrlServiceTests
{
    private AppDbContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task CreateAsync_Should_Create_ShortUrl()
    {
        var db = CreateDb();
        var service = new UrlService(db);

        var result = await service.CreateAsync("https://google.com");

        result.ShortCode.Should().NotBeNullOrEmpty();
        result.OriginalUrl.Should().Be("https://google.com");
        result.ClickCount.Should().Be(0);
    }

    [Fact]
    public async Task CreateAsync_Should_Generate_Unique_Codes()
    {
        var db = CreateDb();
        var service = new UrlService(db);

        var codes = new HashSet<string>();

        for (int i = 0; i < 1000; i++)
        {
            var r = await service.CreateAsync($"https://example.com/{i}");
            codes.Add(r.ShortCode);
        }

        codes.Count.Should().Be(1000);
    }
}