using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using test_Avtobus1.Models;

namespace test_Avtobus1.Services;

public class UrlService
{
    private readonly AppDbContext _db;

    private const string Alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    public UrlService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ShortUrl> CreateAsync(string originalUrl)
    {
        var shortCode = await GenerateUniqueCodeAsync();

        var entity = new ShortUrl
        {
            OriginalUrl = originalUrl,
            ShortCode = shortCode,
            CreatedAt = DateTime.UtcNow,
            ClickCount = 0
        };

        _db.ShortUrls.Add(entity);
        await _db.SaveChangesAsync();

        return entity;
    }

    private async Task<string> GenerateUniqueCodeAsync(int length = 7)
    {
        while (true)
        {
            var code = GenerateRandomCode(length);

            var exists = await _db.ShortUrls
                .AsNoTracking()
                .AnyAsync(x => x.ShortCode == code);

            if (!exists)
                return code;
        }
    }

    private string GenerateRandomCode(int length)
    {
        var bytes = RandomNumberGenerator.GetBytes(length);
        var sb = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            sb.Append(Alphabet[bytes[i] % Alphabet.Length]);
        }

        return sb.ToString();
    }
}
