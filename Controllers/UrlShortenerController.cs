using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Models;
using UrlShortenerApi.Services;

namespace UrlShortenerApi.Controllers;

[ApiController]
[Route("api")]
public class UrlShortenerController : ControllerBase
{

    private readonly ILogger<UrlShortenerController> _logger;
    private readonly UrlShorteningService _urlShorteningService;
    private readonly ApplicationDbContext _dbContext;



    public UrlShortenerController(ILogger<UrlShortenerController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
        _urlShorteningService = new UrlShorteningService(_dbContext);
    }

    [HttpPost("shorten")]
    
    public async Task<object> Post(ShortenUrlRequest request)
    {

        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
        {
            return Results.BadRequest("The specified URL is not valid");
        }

        var code = await _urlShorteningService.GenerateUniqueCode();

        var shortenedUrl = new ShortenedUrl
        {
            Id = Guid.NewGuid(),
            LongUrl = request.Url,
            Code = code,
            ShortUrl = $"{Request.Scheme}://{Request.Host}/api/{code}",
            CreatedOn = DateTime.Now
        };
        _dbContext.ShortenedUrls.Add(shortenedUrl);

        await _dbContext.SaveChangesAsync();

        ShortenUrlResponse response = new ShortenUrlResponse(){
            Link = shortenedUrl.ShortUrl
        };

        return Results.Ok(JsonSerializer.Serialize(response));
    }

    [HttpGet("{code}")]
    public async Task<object> GetNewLink(string code)
    {
        Console.WriteLine("Entra nel GetNewLink");
        var shortenedUrl = await _dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.Code.Equals(code));
        Console.WriteLine($"ShortenedUrl: {shortenedUrl}");

        if (shortenedUrl is null) return Results.NotFound();

        Console.WriteLine($"ShortenedUrl trovato");

        return Results.Redirect(shortenedUrl.LongUrl);
    }

}
