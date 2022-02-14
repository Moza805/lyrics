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

        public LyricsController(IArtistService artistService, ILyricsService lyricsService)
        {
            _artistService = artistService;
            _lyricsService = lyricsService;
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
    }
}
