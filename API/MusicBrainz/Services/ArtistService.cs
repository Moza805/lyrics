using Lyrics.Common.Exceptions;
using Lyrics.Common.Interfaces;
using Lyrics.Common.Models;
using Lyrics.MusicBrainz.Constants;
using Lyrics.MusicBrainz.Models;
using System.Net;
using System.Net.Http.Json;
using System.Web;

namespace Lyrics.MusicBrainz.Services
{
    /// <summary>
    /// Provides information on artists. Data is sourced from MusicBrainz API
    /// </summary>
    /// <seealso cref="https://musicbrainz.org/doc/MusicBrainz_API"/>
    public sealed class ArtistService : IArtistService
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// New up an instance of this service
        /// </summary>
        /// <param name="httpClient"></param>
        public ArtistService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Find artists whos names are a close match to <paramref name="name"/>
        /// </summary>
        /// <param name="name">Artist to search for</param>
        /// <returns>A collection of artists that match the search term</returns>
        public async Task<IEnumerable<Common.Models.Artist>> FindArtistsByNameAsync(string name)
        {
            var url = $"ws/2/artist/?query=artist:{HttpUtility.UrlEncode(name)}";

            try
            {
                var responseData = await _httpClient.GetFromJsonAsync<ArtistSearchResponse>(url);

                return responseData.Artists.Select((artist) => new Common.Models.Artist(artist.Id, artist.Name, artist.Type, artist.Disambiguation));
            }
            catch (Exception ex)
            {
                throw new ThirdPartyServiceException("Failed to call MusicBrainz API", ex);
            }
        }

        /// <summary>
        /// Get all songs by an artist
        /// </summary>
        /// <param name="artistId">Globally unique identifier for the artist in whichever system you are sourcing data from</param>
        /// <returns>A collection of songs or empty list if no match on <paramref name="artistId"/></returns>
        /// <exception cref="ThirdPartyServiceException">MusicBrainz server problems, see inner exception.</exception>
        public async Task<IEnumerable<Song>> GetSongsByArtistAsync(Guid artistId)
        {
            var url = $"ws/2/work/?artist={artistId}&limit=1000&inc=artist-rels";

            try
            {
                var responseData = await _httpClient.GetFromJsonAsync<SongListResponse>(url);

                return responseData.Works.Where((work) => work.Type == WorkConstants.WorkTypes.Song).Select((work) => new Song(work.Id, work.Title));
            }
            catch (HttpRequestException ex)
            {
                switch (ex.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new List<Song>();
                    case HttpStatusCode.InternalServerError:
                    default:
                        throw new ThirdPartyServiceException("Failed to call MusicBrainz API", ex);
                }
            }
        }

        /// <summary>
        /// Get details about a given artist
        /// </summary>
        /// <param name="artistId">Globally unique identifier for the artist in whichever system you are sourcing data from</param>
        /// <returns>Artist details</returns>
        /// <exception cref="ArtistNotFoundException">No artist found that matches provided <paramref name="artistId"/></exception>
        /// <exception cref="ThirdPartyServiceException">A third party API did not respond as expected</exception>
        public async Task<Common.Models.Artist> GetArtistByIdAsync(Guid artistId)
        {
            var url = $"ws/2/artist/{artistId}";

            try
            {
                var responseData = await _httpClient.GetFromJsonAsync<ArtistResponse>(url);
                return new Common.Models.Artist(responseData.Id, responseData.Name, responseData.Type, responseData.Disambiguation);
            }
            catch (HttpRequestException ex)
            {
                switch (ex.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        throw new ArtistNotFoundException(artistId);
                    case HttpStatusCode.InternalServerError:
                    default:
                        throw new ThirdPartyServiceException("Failed to call MusicBrainz API", ex);
                }
            }
        }
    }
}
