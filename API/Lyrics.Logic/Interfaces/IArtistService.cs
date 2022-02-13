using Lyrics.Logic.Models;

namespace Lyrics.Logic.Interfaces
{
    public interface IArtistService
    {
        IEnumerable<Artist> FindArtistsByName(string name);
    }
}
