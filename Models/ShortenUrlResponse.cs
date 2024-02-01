using System.Text.Json.Serialization;

namespace UrlShortenerApi.Models;

public class ShortenUrlResponse
{
        [JsonPropertyName("link")]
        public string Link {get;set;} = string.Empty;
}