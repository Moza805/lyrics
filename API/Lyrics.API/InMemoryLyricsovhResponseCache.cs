using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Lyrics.API
{
    public class InMemoryLyricsovhResponseCache : DelegatingHandler
    {
        private readonly IMemoryCache _cache;

        public InMemoryLyricsovhResponseCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Check if url in cache
            if (_cache.TryGetValue(request.RequestUri.AbsolutePath, out string cachedLyrics))
            {
                // Return lyrics from cache
                return new HttpResponseMessage(HttpStatusCode.OK) { Content = JsonContent.Create(new LyricsResponse { Lyrics = cachedLyrics }) };
            }

            // No cache hit, fetch lyrics over network
            var response = await base.SendAsync(request, cancellationToken);

            // If we found lyrics, store them.
            // Dont cache for 404, we probably want to keep checking every time in case they've been added
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var lyricsToCache = JsonSerializer.Deserialize<LyricsResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions(JsonSerializerDefaults.Web));
                _cache.Set(request.RequestUri.AbsolutePath, lyricsToCache.Lyrics);
            }

            return response;
        }

        internal class LyricsResponse
        {
            public string Lyrics { get; set; }
        }


    }
}
