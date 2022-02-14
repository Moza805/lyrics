using Lyrics.Common.Exceptions;
using Lyrics.Common.Interfaces;
using Lyrics.Common.Models;

namespace Lyrics.Logic.Services
{
    /// <summary>
    /// A service for getting stats about an artists catalogue of songs
    /// </summary>
    public class StatisticsService : IStatisticsService
    {
        private readonly IArtistService _artistService;
        private readonly ILyricsService _lyricsService;

        public StatisticsService(IArtistService artistService, ILyricsService lyricsService)
        {
            _artistService = artistService;
            _lyricsService = lyricsService;
        }

        /// <summary>
        /// Get statistics for a given artist
        /// </summary>
        /// <param name="artistId">Artist ID</param>
        /// <returns>Statistics such as average, longest, shortest song etc</returns>
        /// <exception cref="ArtistNotFoundException">No artist found that matches provided <paramref name="artistId"/></exception>
        /// <exception cref="ThirdPartyServiceException">A third party API did not respond as expected</exception>
        public async Task<ArtistStatistics> GetStatistics(Guid artistId)
        {
            var artist = await _artistService.GetArtistByIdAsync(artistId);
            var songsByArtist = await _artistService.GetSongsByArtistAsync(artistId);

            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 4
            };

            var getLyricsTasks = songsByArtist.Select(
                (song) => Task.Run(
                    async () => new Song(song.Id, song.Title)
                    {
                        Lyrics = await _lyricsService.GetLyricsForSongAsync(artist.Name, song.Title)
                    }
                )
            );

            await Task.WhenAll(getLyricsTasks);

            var songsWithLyrics = getLyricsTasks.Select(x => x.Result);

            return new ArtistStatistics(artist, songsWithLyrics.ToArray());
        }
    }
}