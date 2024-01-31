using Microsoft.EntityFrameworkCore;

namespace UrlShortenerApi.Services;

public class UrlShorteningService
{
    public const int NumbersOfCharInShortLinks = 7;
    public const string Alphabet =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    private readonly Random _random = new();
    private readonly ApplicationDbContext? _dbContext;

    public UrlShorteningService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<string> GenerateUniqueCode()
    {
        char[] codeChars = new char[NumbersOfCharInShortLinks];


        while (true)
        {
            for (int i = 0; i < NumbersOfCharInShortLinks; i++)
            {

                int randomIndex = _random.Next(Alphabet.Length - 1);

                codeChars[i] = Alphabet[randomIndex];
            }

            var code = new string(codeChars);

            if (!await _dbContext.ShortenedUrls.AnyAsync(s => s.Code.Equals(code)))
                return code;
        }
    }

}