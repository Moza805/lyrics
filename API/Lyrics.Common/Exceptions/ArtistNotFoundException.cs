namespace Lyrics.Common.Exceptions
{
    /// <summary>
    /// Thrown when there is no artist found in the given data source for provided ID
    /// </summary>
    public class ArtistNotFoundException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="artistId">Globally unique ID in the given data source</param>
        public ArtistNotFoundException(Guid artistId) : base($"Could not find an artist for the ID {artistId}")
        {

        }
    }
}
