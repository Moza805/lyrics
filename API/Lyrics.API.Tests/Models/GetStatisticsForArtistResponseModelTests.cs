using Lyrics.API.Models;
using Lyrics.Common.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lyrics.API.Tests.Models
{
    public class GetStatisticsForArtistResponseModelTests
    {
        [Test]
        public void GetStatistsForArtistResponseModel_SetsAllPropertiesCorrectly()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var shortSongGuid = Guid.NewGuid();
            var longSongGuid = Guid.NewGuid();

            var artistStatistics = new ArtistStatistics(
                new Artist(artistGuid, "Jaco Pastorius", "Person", "Bass player extraordinaire"),
                new Song[]
                {
                    new Song(shortSongGuid, "Come On, Come Over") { Lyrics ="Come on, come over"},
                    new Song(longSongGuid, "Another song") { Lyrics = "This song isn't real but it has more words" },
                    new Song(Guid.NewGuid(), "Hey look another song") { Lyrics = "Not so short not so long"}
                });

            // Test
            var result = new GetStatisticsForArtistResponseModel(artistStatistics);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(artistGuid, result.Artist.Id);
            Assert.AreEqual(shortSongGuid, result.ShortestSong.Id);
            Assert.AreEqual(longSongGuid, result.LongestSong.Id);
            Assert.AreEqual(3, result.NumberOfSongs);
            Assert.IsTrue(Math.Abs(4.223d - result.Variance) < 0.01);
            Assert.IsTrue(Math.Abs(6.333 - result.AverageWordsPerSong) < 0.01);
            Assert.IsTrue(Math.Abs(2.05 - result.StandardDeviation) < 0.01);
        }
    }
}
