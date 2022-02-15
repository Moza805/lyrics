using Lyrics.Common.Models;

namespace Lyrics.API.Models
{
    public class GetStatisticsForArtistResponseModel
    {
        public Artist Artist { get; set; }
        public int NumberOfSongs { get; set; }

        /// <summary>
        /// The song with the most words in it
        /// </summary>
        public Song LongestSong { get; set; }


        /// <summary>
        /// The song with the least words in it
        /// </summary>
        public Song ShortestSong { get; set; }

        /// <summary>
        /// Mean number of words per song
        /// </summary>
        public double AverageWordsPerSong { get; set; }

        /// <summary>
        /// Variance of word count per song
        /// </summary>
        public double Variance { get; set; }

        /// <summary>
        /// Standard deviation of word count per song
        /// </summary>
        public double StandardDeviation { get; set; }

        public GetStatisticsForArtistResponseModel(ArtistStatistics artistStatistics)
        {
            Artist = artistStatistics.Artist;
            NumberOfSongs = artistStatistics.Songs.Length;
            LongestSong = artistStatistics.LongestSong;
            ShortestSong = artistStatistics.ShortestSong;
            AverageWordsPerSong = artistStatistics.AverageWordsPerSong;
            Variance = artistStatistics.Variance;
            StandardDeviation = artistStatistics.StandardDeviation;
        }
    }
}
