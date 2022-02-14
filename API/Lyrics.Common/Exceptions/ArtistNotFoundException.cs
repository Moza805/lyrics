namespace Lyrics.Common.Exceptions
{
    public class ArtistNotFoundException : Exception
    {
        public ArtistNotFoundException(Guid artistId) : base($"Could not find an artist for the ID {artistId}")
        {

        }
    }
}
