using Lyrics.Common.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrics.Common.Tests.Models
{
    public class ArtistStatisticsTests
    {
        [Test]
        public void LongestSong_ReturnsLongestSong()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                new[]
                {
                    new Song(Guid.NewGuid(), "First song") { Lyrics = "A song with a lot of words in it" },
                    new Song(Guid.NewGuid(), "Second song"){ Lyrics = "Not so many words" }
                });

            // Test
            var longestSong = statisticsUnderTest.LongestSong;

            // Assert
            Assert.That(longestSong.Id, Is.EqualTo(statisticsUnderTest.Songs[0].Id));
        }

        [Test]
        public void ShortestSong_ReturnsShortestSong()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                new[]
                {
                    new Song(Guid.NewGuid(), "First song") { Lyrics = "A song with a lot of words in it" },
                    new Song(Guid.NewGuid(), "Second song"){ Lyrics = "Not so many words" }
                });

            // Test
            var shortestSong = statisticsUnderTest.ShortestSong;

            // Assert
            Assert.That(shortestSong.Id, Is.EqualTo(statisticsUnderTest.Songs[1].Id));
        }

        [Test]
        public void LongestSong_ReturnsNullWhenNoSongs()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                Array.Empty<Song>());

            // Test
            var longestSong = statisticsUnderTest.LongestSong;

            // Assert
            Assert.IsNull(longestSong);
        }

        [Test]
        public void ShortestSong_ReturnsNullWhenNoSongs()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                Array.Empty<Song>());

            // Test
            var shortestSong = statisticsUnderTest.ShortestSong;

            // Assert
            Assert.IsNull(shortestSong);
        }

        [Test]
        public void AverageWordsPerSong_ReturnsZeroWhenNoSongs()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                Array.Empty<Song>());

            // Test
            var average = statisticsUnderTest.AverageWordsPerSong;

            // Assert
            Assert.That(average, Is.EqualTo(0));

        }

        [Test]
        public void AverageWordsPerSong_ReturnsCorrectNumber()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                new[]
                {
                    new Song(Guid.NewGuid(), "First song") { Lyrics = "A four word song" },
                    new Song(Guid.NewGuid(), "Second song"){ Lyrics = "A song that has eight words in it" }
                });

            // Test
            var average = statisticsUnderTest.AverageWordsPerSong;

            // Assert
            Assert.That(average, Is.EqualTo(6d));
        }

        [Test]
        public void AverageWordsPerSong_WorksWithOneSong()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                new[]
                {
                    new Song(Guid.NewGuid(), "First song") { Lyrics = "A four word song" }
                });

            // Test
            var average = statisticsUnderTest.AverageWordsPerSong;

            // Assert
            Assert.That(average, Is.EqualTo(4d));
        }

        [Test]
        public void AverageWordsPerSong_IgnoresSongsWithoutLyrics()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                new[]
                {
                    new Song(Guid.NewGuid(), "First song") { Lyrics = "A four word song" },
                    new Song(Guid.NewGuid(), "Second song")
                });

            // Test
            var average = statisticsUnderTest.AverageWordsPerSong;

            // Assert
            Assert.That(average, Is.EqualTo(4d));
        }

        [Test]
        public void Variance_WorksWhenTheresNoSongs()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                Array.Empty<Song>());

            // Test
            var variance = statisticsUnderTest.Variance;

            // Assert
            Assert.That(variance, Is.EqualTo(0));
        }

        [Test]
        public void Variance_WorksWhenThereIsOneSong()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                new[]
                {
                    new Song(Guid.NewGuid(), "First song") { Lyrics = "A four word song" }
                });

            // Test
            var variance = statisticsUnderTest.Variance;

            // Assert
            Assert.That(variance, Is.EqualTo(0));
        }

        [Test]
        public void Variance_WorksWhenThereIsMoreThanOneSong()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                new[]
                {
                    new Song(Guid.NewGuid(), "First song") { Lyrics = "A four word song" },
                    new Song(Guid.NewGuid(), "Second song") { Lyrics = "A song with lots of words to make variance" }
                });

            // Test
            var variance = statisticsUnderTest.Variance;

            // Assert
            Assert.That(variance, Is.EqualTo(6.25d));
        }

        [Test]
        public void Variance_IgnoresSongsWithNoLyrics()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                new[]
                {
                    new Song(Guid.NewGuid(), "First song") { Lyrics = "A four word song" },
                    new Song(Guid.NewGuid(), "Second song") { Lyrics = "A song with lots of words to make variance" },
                    new Song(Guid.NewGuid(), "Third song")
                });

            // Test
            var variance = statisticsUnderTest.Variance;

            // Assert
            Assert.That(variance, Is.EqualTo(6.25d));
        }

        [Test]
        public void StandardDeviation_WorksWhenTheresNoSongs()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                Array.Empty<Song>());

            // Test
            var standardDeviation = statisticsUnderTest.StandardDeviation;

            // Assert
            Assert.That(standardDeviation, Is.EqualTo(0));
        }

        [Test]
        public void StandardDeviation_WorksWhenThereIsOneSong()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                new[]
                {
                    new Song(Guid.NewGuid(), "First song") { Lyrics = "A four word song" }
                });

            // Test
            var standardDeviation = statisticsUnderTest.StandardDeviation;

            // Assert
            Assert.That(standardDeviation, Is.EqualTo(0));
        }

        [Test]
        public void StandardDeviation_WorksWhenThereIsMoreThanOneSong()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                new[]
                {
                    new Song(Guid.NewGuid(), "First song") { Lyrics = "A four word song" },
                    new Song(Guid.NewGuid(), "Second song") { Lyrics = "A song with lots of words to make variance" }
                });

            // Test
            var standardDeviation = statisticsUnderTest.StandardDeviation;

            // Assert
            Assert.That(standardDeviation, Is.EqualTo(2.5d));
        }

        [Test]
        public void StandardDeviation_IgnoresSongsWithNoLyrics()
        {
            // Setup
            var statisticsUnderTest = new ArtistStatistics(
                new Artist(Guid.NewGuid(), "Mutemath", "Group", null),
                new[]
                {
                    new Song(Guid.NewGuid(), "First song") { Lyrics = "A four word song" },
                    new Song(Guid.NewGuid(), "Second song") { Lyrics = "A song with lots of words to make variance" },
                    new Song(Guid.NewGuid(), "Third song")
                });

            // Test
            var standardDeviation = statisticsUnderTest.StandardDeviation;

            // Assert
            Assert.That(standardDeviation, Is.EqualTo(2.5d));
        }
    }
}
