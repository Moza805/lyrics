using Lyrics.API.Models;
using Lyrics.Common.Exceptions;
using Lyrics.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Lyrics.API.Controllers
{
    [ApiController]
    [Route("/")]
    public class LyricsController : ControllerBase
    {
        private readonly IArtistService _artistService;
        private readonly ILyricsService _lyricsService;
        private readonly IStatisticsService _statisticsService;

        public LyricsController(IArtistService artistService, ILyricsService lyricsService, IStatisticsService statisticsService)
        {
            _artistService = artistService;
            _lyricsService = lyricsService;
            _statisticsService = statisticsService;
        }

        [HttpGet("SearchArtistsByName/{artist}")]
        public async Task<IActionResult> SearchArtistsByNameAsync(string artist)
        {
            try
            {
                var result = await _artistService.FindArtistsByNameAsync(artist);
                return Ok(result);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetSongsForArtist/{artistId}")]
        public async Task<IActionResult> GetSongsForArtistAsync(Guid artistId)
        {
            try
            {
                var result = await _artistService.GetSongsByArtistAsync(artistId);
                return Ok(result);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetLyricsForSong/{artist}/{song}")]
        public async Task<IActionResult> GetLyricsForSong(string artist, string song)
        {
            try
            {
                var result = await _lyricsService.GetLyricsForSongAsync(artist, song);
                return Ok(result);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet("GetStatisticsForArtist/{artistId}")]
        public async Task<IActionResult> GetStatisticsForArtistAsync(Guid artistId)
        {
            try
            {
                var result = await _statisticsService.GetStatistics(artistId);
                return Ok(new GetStatisticsForArtistResponseModel(result));
            }
            catch (ArtistNotFoundException)
            {
                return StatusCode((int)HttpStatusCode.NotFound);
            }
            catch
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
