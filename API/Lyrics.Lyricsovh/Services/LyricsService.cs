using Lyrics.Common.Exceptions;
using Lyrics.Common.Interfaces;
using Lyrics.Lyricsovh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lyrics.Lyricsovh.Services
{
    public sealed class LyricsService : ILyricsService
    {
        private readonly HttpClient _client;

        JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        public LyricsService(HttpClient client)
        {
            _client = client;
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
            var url = $"https://private-anon-1716d3eb21-lyricsovh.apiary-proxy.com/v1/{artist}/{song}";

            try
            {
                var httpResponse = await _client.GetAsync(url);

                httpResponse.EnsureSuccessStatusCode();
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                var responseData = JsonSerializer.Deserialize<GetLyricsResponse>(responseString, _jsonSerializerOptions);

                return responseData.Lyrics;
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
