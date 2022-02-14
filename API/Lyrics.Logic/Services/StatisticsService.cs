using Lyrics.Common.Interfaces;
using Lyrics.Common.Models;

namespace Lyrics.Logic.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IArtistService _artistService;
        private readonly ILyricsService _lyricsService;

        public StatisticsService(IArtistService artistService, ILyricsService lyricsService)
        {
            _artistService = artistService;
            _lyricsService = lyricsService;
        }

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