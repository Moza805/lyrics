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

        public LyricsController(IArtistService artistService)
        {
            _artistService = artistService;
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
    }
}
