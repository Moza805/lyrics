using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrics.Common.Models
{
    /// <summary>
    /// Details about the wordiness of an artists songs
    /// </summary>
    public class ArtistStatistics
    {
        public Artist Artist { get; set; }

        public Song[] Songs { get; set; }

        /// <summary>
        /// Gets the song with the most words in it
        /// </summary>
        public Song LongestSong
        {
            get => Songs.Length > 0
                ? Songs.Aggregate((longest, current) => longest.WordCount > current.WordCount ? longest : current)
                : null;
        }


        /// <summary>
        /// Gets the song with the least words in it
        /// </summary>
        public Song ShortestSong
        {
            get => Songs.Length > 0
                ? Songs.Where((song) => song.WordCount > 0).Aggregate((shortest, current) => shortest.WordCount < current.WordCount ? shortest : current)
                : null;
        }

        /// <summary>
        /// Mean number of words per song
        /// </summary>
        public double AverageWordsPerSong
        {
            get => Songs.Length > 0
                ? Songs.Where((song) => song.WordCount > 0).Select((song) => song.WordCount).Average()
                : 0;
        }

        /// <summary>
        /// Variance of word count per song
        /// </summary>
        public double Variance
        {
            get
            {
                var mean = AverageWordsPerSong;
                // Save a few CPU cycles
                if (mean == 0)
                {
                    return 0;
                }

                var differencesFromMean = Songs.Where((song) => song.WordCount > 0).Select((song) => Math.Pow(song.WordCount - mean, 2));
                return differencesFromMean.Average();
            }
        }

        /// <summary>
        /// Standard deviation of word count per song
        /// </summary>
        public double StandardDeviation
        {
            get => Math.Sqrt(Variance);
        }

        public ArtistStatistics(Artist artist, Song[] songs)
        {
            Artist = artist;
            Songs = songs;
        }
    }
}
