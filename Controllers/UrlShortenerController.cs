using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
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
    private readonly IMemoryCache _cache;



    public UrlShortenerController(ILogger<UrlShortenerController> logger, ApplicationDbContext dbContext, IMemoryCache cache)
    {
        _logger = logger;
        _dbContext = dbContext;
        _urlShorteningService = new UrlShorteningService(_dbContext);
        _cache = cache;
    }

    [HttpPost("shorten")]
    public async Task<object> Post(ShortenUrlRequest request)
    {
        _logger.LogInformation("Start POST Method");
        if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
        {
            return Results.BadRequest("The specified URL is not valid");
        }

        var code = await _urlShorteningService.GenerateUniqueCode();
        _logger.LogInformation($"Unique Code Generated: {code}");


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

        ShortenUrlResponse response = new ShortenUrlResponse()
        {
            Link = shortenedUrl.ShortUrl
        };

        return Results.Ok(JsonSerializer.Serialize(response));
    }

    [HttpGet("{code}")]
    public async Task<object> GetNewLink(string code)
    {
        _logger.LogInformation($"Start Get Method");

        ShortenedUrl? shortenedUrl = new ShortenedUrl();

        if (_cache.TryGetValue(code, out shortenedUrl))
            _logger.LogInformation("Url found in cache");
        else
        {
            _logger.LogInformation("Url not found in cache");
            shortenedUrl = await _dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.Code.Equals(code));
            _logger.LogInformation($"ShortenedUrl: {shortenedUrl}");

            _cache.Set(code, shortenedUrl);
            _logger.LogInformation("Url added to cache");

        }

        if (shortenedUrl is null) return Results.NotFound();

        _logger.LogInformation($"Find Record: {shortenedUrl.LongUrl}");
        return Results.Redirect(shortenedUrl.LongUrl);
    }

}
