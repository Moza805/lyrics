using Lyrics.Common.Models;

namespace Lyrics.Common.Interfaces
{
    /// <summary>
    /// Provides information on artists. Data is sourced from MusicBrainz API via <see cref="https://github.com/Zastai/MetaBrainz.MusicBrainz"/>
    /// </summary>
    /// <seealso cref="https://musicbrainz.org/doc/MusicBrainz_API"/>
    public interface IArtistService
    {

        /// <summary>
        /// Find artists whos names are a close match to <paramref name="name"/>
        /// </summary>
        /// <param name="name">Artist to search for</param>
        /// <returns>A collection of artists that match the search term</returns>
        Task<IEnumerable<Artist>> FindArtistsByNameAsync(string name);
    }
}
