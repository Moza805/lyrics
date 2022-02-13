using Lyrics.Common.Exceptions;
using Lyrics.Common.Interfaces;
using Lyrics.Common.Models;
using Lyrics.MusicBrainz.Models;
using System.Text.Json;
using System.Web;

namespace Lyrics.MusicBrainz.Services
{
    /// <summary>
    /// Provides information on artists. Data is sourced from MusicBrainz API
    /// </summary>
    /// <seealso cref="https://musicbrainz.org/doc/MusicBrainz_API"/>
    public class ArtistService : IArtistService
    {
        private readonly string _applicationName;
        private readonly Version _version;
        private readonly string _contactEmail;
        private readonly HttpClient _httpClient;

        /// <summary>
        /// New up an instance of this service
        /// </summary>
        /// <param name="applicationName">How your application should present to the MusicBrainz API</param>
        /// <param name="version">Version of calling assembly</param>
        /// <param name="contactEmail">Contact details to pass to MusicBrainz</param>
        public ArtistService(string applicationName, Version version, string contactEmail, HttpClient httpClient)
        {
            _applicationName = applicationName;
            _version = version;
            _contactEmail = contactEmail;
            _httpClient = httpClient;

            httpClient.DefaultRequestHeaders.Add("User-Agent", $"{applicationName}/${version} (${_contactEmail})");
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        /// <summary>
        /// Find artists whos names are a close match to <paramref name="name"/>
        /// </summary>
        /// <param name="name">Artist to search for</param>
        /// <returns>A collection of artists that match the search term</returns>
        public async Task<IEnumerable<Common.Models.Artist>> FindArtistsByNameAsync(string name)
        {
            var url = $"https://musicbrainz.org/ws/2/artist/?query=artist:{HttpUtility.UrlEncode(name)}";

            using (HttpResponseMessage httpResponse = _httpClient.GetAsync(url).Result)
            { 
                try
                {
                    httpResponse.EnsureSuccessStatusCode();
                    var responseString = await httpResponse.Content.ReadAsStringAsync();
                    var responseData = JsonSerializer.Deserialize<ArtistSearchResponse>(responseString, new JsonSerializerOptions(JsonSerializerDefaults.Web));

                    return responseData.Artists.Select((artist) => new Common.Models.Artist(artist.Id, artist.Name, artist.Type, artist.Disambiguation));
                }
                catch (Exception ex)
                {
                    throw new ThirdPartyServiceException("Failed to call MusicBrainz API", ex);
                }
            }
        }
    }
}
