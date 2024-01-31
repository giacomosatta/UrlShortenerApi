using System.Security.Permissions;
using Microsoft.AspNetCore.Mvc;
using UrlShortenerApi.Models;
using UrlShortenerApi.Services;

namespace UrlShortenerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UrlShortenerController : ControllerBase
{

    private readonly ILogger<UrlShortenerController> _logger;
    private readonly UrlShorteningService _urlShorteningService;
    private readonly ApplicationDbContext _dbContext;
    private readonly HttpContext _httpContext;



//     public UrlShortenerController(ILogger<UrlShortenerController> logger,ApplicationDbContext dbContext,HttpContext httpContext)
//     {
//         _logger = logger;
//         _urlShorteningService = new UrlShorteningService();
//         _dbContext = new ApplicationDbContext();
//         _httpContext = new HttpContext();
//     }

//     [HttpPost(Name = "api/shorten")]
//     public async Task<object> Post(ShortenUrlRequest request)
//     {

//         if(!Uri.TryCreate(request.Url,UriKind.Absolute, out _))
//         {
//             return Results.BadRequest("The specified URL is not valid");
//         }
      
// var code = await urlShorteningService.GenerateUniqueCode();

//       var shortenedUrl = new ShortenedUrl{
//         Id = Guid.NewGuid(),
//         LongUrl = request.Url,
//         Code = code,
//         ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
//         CreatedOn = DateTime.Now
//       };
//       dbContext.ShortenedUrls.Add(shortenedUrl);

//       await dbContext.SaveChangesAsync();

//       return Results.Ok(shortenedUrl.ShortUrl);
//     }
}
