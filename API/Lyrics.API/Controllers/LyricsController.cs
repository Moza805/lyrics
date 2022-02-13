using Lyrics.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Lyrics.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LyricsController : ControllerBase
    {
        private readonly IArtistService _artistService;

        public LyricsController(IArtistService artistService)
        {
            _artistService = artistService;
        }

        [HttpGet("SearchByName/{artist}")]
        public async Task<IActionResult> SearchByName(string artist)
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
    }
}
