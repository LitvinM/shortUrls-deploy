using Microsoft.EntityFrameworkCore;
using test_Avtobus1.Models;

namespace test_Avtobus1;

public class AppDbContext : DbContext
{
    public DbSet<ShortUrl> ShortUrls => Set<ShortUrl>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortUrl>()
            .HasIndex(x => x.ShortCode)
            .IsUnique();
    }
}
