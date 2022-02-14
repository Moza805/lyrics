namespace Lyrics.Common.Interfaces
{
    /// <summary>
    /// Services related to getting lyrics for a song
    /// </summary>
    public interface ILyricsService
    {
        /// <summary>
        /// Get lyrics for a song
        /// </summary>
        /// <param name="artist">Artist e.g. Vulfpeck</param>
        /// <param name="song">Song name e.g. Back Pocket</param>
        /// <returns>Song lyrics, line terminator \r\n</returns>
        Task<string> GetLyricsForSongAsync(string artist, string song);
    }
}
