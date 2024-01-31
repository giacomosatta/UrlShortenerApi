using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Models;
using UrlShortenerApi.Services;

namespace UrlShortenerApi;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {

    }

    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ShortenedUrl>(builder =>
        {
            builder.Property(s => s.Code).HasMaxLength(UrlShorteningService.NumbersOfCharInShortLinks);
            builder.HasIndex(s => s.Code).IsUnique();
        });
    }
}