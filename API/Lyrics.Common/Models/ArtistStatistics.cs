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
            get
            {
                var songsWithLyrics = Songs.Where((song) => song.WordCount > 0);
                if (songsWithLyrics.Count() == 0)
                {
                    return null;
                }

                return Songs.Aggregate((longest, current) => longest.WordCount > current.WordCount ? longest : current);
            }
        }


        /// <summary>
        /// Gets the song with the least words in it
        /// </summary>
        public Song ShortestSong
        {
            get {
                var songsWithLyrics = Songs.Where((song) => song.WordCount > 0);
                if(songsWithLyrics.Count() == 0)
                {
                    return null;
                }

                return Songs.Where((song) => song.WordCount > 0).Aggregate((shortest, current) => shortest.WordCount < current.WordCount ? shortest : current);
            }
        }

        /// <summary>
        /// Mean number of words per song
        /// </summary>
        public double AverageWordsPerSong
        {
            get
            {
                var songsWithLyrics = Songs.Where((song) => song.WordCount > 0);
                if (songsWithLyrics.Count() == 0)
                {
                    return 0d;
                }
                return Songs.Where((song) => song.WordCount > 0).Select((song) => song.WordCount).Average();
            }
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
                    return 0d;
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
