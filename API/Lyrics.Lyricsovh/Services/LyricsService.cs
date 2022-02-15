using Lyrics.Common.Exceptions;
using Lyrics.Common.Interfaces;
using Lyrics.Lyricsovh.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.RegularExpressions;

namespace Lyrics.Lyricsovh.Services
{
    public sealed class LyricsService : ILyricsService
    {
        private readonly HttpClient _client;

        /// <summary>
        /// New up an instance of this service
        /// </summary>
        /// <param name="httpClient"></param>
        public LyricsService(HttpClient httpClient)
        {
            _client = httpClient;
        }

        /// <summary>
        /// Get lyrics to a song
        /// </summary>
        /// <param name="artist">Artist of the song</param>
        /// <param name="song">Name of the song</param>
        /// <returns>Lyrics of the song line terminated with \r\n or empty string if no match</returns>
        /// <exception cref="ThirdPartyServiceException">Lyricsovh server error</exception>
        public async Task<string> GetLyricsForSongAsync(string artist, string song)
        {
            var url = $"v1/{artist}/{song}";

            try
            {
                var responseData = await _client.GetFromJsonAsync<GetLyricsResponse>(url);
                return Regex.Replace(responseData.Lyrics, @"^P.*\r\n", "");
            }
            catch (HttpRequestException ex)
            {
                switch (ex.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return string.Empty;
                    case HttpStatusCode.InternalServerError:
                    default:
                        throw new ThirdPartyServiceException("Failed to call MusicBrainz API", ex);
                }
            }
        }
    }
}
