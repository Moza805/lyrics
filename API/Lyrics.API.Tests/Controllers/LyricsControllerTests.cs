using Lyrics.API.Controllers;
using Lyrics.API.Models;
using Lyrics.Common.Exceptions;
using Lyrics.Common.Interfaces;
using Lyrics.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Lyrics.API.Tests.Controllers
{
    public class LyricsControllerTests
    {
        readonly Mock<IArtistService> _artistServiceMock = new();
        readonly Mock<ILyricsService> _lyricsServiceMock = new();
        readonly Mock<IStatisticsService> _statisticsServiceMock = new();

        #region SearchArtistsByNameAsync

        [Test]
        public async Task SearchArtistsByNameAsync_ReturnsDataRetrievedFromService()
        {
            // Setup
            var serviceResults = new List<Artist>
            {
                new Artist(Guid.NewGuid(),"Henge","band","Really good live"),
                new Artist(Guid.NewGuid(),"Henge","person", "Not even real")
            };
            _artistServiceMock.Setup(x => x.FindArtistsByNameAsync("Henge")).ReturnsAsync(serviceResults).Verifiable();

            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.SearchArtistsByNameAsync("Henge") as OkObjectResult;

            // Assert
            _artistServiceMock.Verify();
            Assert.That(result.Value, NUnit.DeepObjectCompare.Is.DeepEqualTo(serviceResults));
        }

        [Test]
        public async Task SearchArtistsByNameAsync_ReturnsOk()
        {
            // Setup
            var serviceResults = new List<Artist>
            {
                new Artist(Guid.NewGuid(),"Henge","band","Really good live"),
                new Artist(Guid.NewGuid(),"Henge","person", "Not even real")
            };
            _artistServiceMock.Setup(x => x.FindArtistsByNameAsync("Henge")).ReturnsAsync(serviceResults);

            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.SearchArtistsByNameAsync("Henge") as OkObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public async Task SearchArtistsByNameAsync_Returns500ForServiceException()
        {
            // Setup
            _artistServiceMock.Setup(x => x.FindArtistsByNameAsync("Henge")).ThrowsAsync(new ThirdPartyServiceException("Rate limited!", new Exception("Too many requests")));

            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.SearchArtistsByNameAsync("Henge") as StatusCodeResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        #endregion

        #region GetSongsForArtistAsync

        [Test]
        public async Task GetSongsForArtistAsync_ReturnsDataRetrievedFromService()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var serviceResults = new List<Song>
            {
                new Song(Guid.NewGuid(),"Suro Nipa"),
                new Song(Guid.NewGuid(),"Shidaa")
            };
            _artistServiceMock.Setup(x => x.GetSongsByArtistAsync(artistGuid)).ReturnsAsync(serviceResults);

            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.GetSongsForArtistAsync(artistGuid) as OkObjectResult;

            // Assert
            _artistServiceMock.Verify();
            Assert.That(result.Value, NUnit.DeepObjectCompare.Is.DeepEqualTo(serviceResults));
        }

        [Test]
        public async Task GetSongsForArtistAsync_ReturnsOk()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var serviceResults = new List<Song>
            {
                new Song(Guid.NewGuid(),"Suro Nipa"),
                new Song(Guid.NewGuid(),"Shidaa")
            };
            _artistServiceMock.Setup(x => x.GetSongsByArtistAsync(artistGuid)).ReturnsAsync(serviceResults);

            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.GetSongsForArtistAsync(artistGuid) as OkObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public async Task GetSongsForArtistAsync_Returns500ForServiceException()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            _artistServiceMock.Setup(x => x.GetSongsByArtistAsync(artistGuid)).ThrowsAsync(new ThirdPartyServiceException("Rate limited!", new Exception("Too many requests")));

            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.GetSongsForArtistAsync(artistGuid) as StatusCodeResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        #endregion

        #region GetSongsForArtistAsync

        [Test]
        public async Task GetLyricsForSongAsync_ReturnsDataRetrievedFromService()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var serviceResults = "These are some lyrics\r\nOn a new line";
            _lyricsServiceMock.Setup(x => x.GetLyricsForSongAsync("Fake artist", "Song title")).ReturnsAsync(serviceResults).Verifiable();

            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.GetLyricsForSong("Fake artist", "Song title") as OkObjectResult;

            // Assert
            _artistServiceMock.Verify();
            Assert.AreEqual(result.Value, serviceResults);
        }

        [Test]
        public async Task GetLyricsForSongAsync_ReturnsOk()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var serviceResults = "These are some lyrics\r\nOn a new line";
            _lyricsServiceMock.Setup(x => x.GetLyricsForSongAsync("Fake artist", "Song title")).ReturnsAsync(serviceResults);

            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.GetLyricsForSong("Fake artist", "Song title") as OkObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public async Task GetLyricsForSongAsync_Returns500ForServiceException()
        {
            // Setup
            var artistGuid = Guid.NewGuid();

            _lyricsServiceMock.Setup(x => x.GetLyricsForSongAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new ThirdPartyServiceException("Rate limited!", new Exception("Too many requests")));

            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.GetLyricsForSong("Fake artist", "Song title") as StatusCodeResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        #endregion

        #region GetStatisticsForArtistAsync


        [Test]
        public async Task GetStatisticsForArtistAsync_ReturnsDataRetrievedFromService()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var shortSongGuid = Guid.NewGuid();
            var longSongGuid = Guid.NewGuid();
            
            var serviceResults = new ArtistStatistics(
                new Artist(artistGuid, "Jaco Pastorius", "Person", "Bass player extraordinaire"),
                new Song[]
                {
                    new Song(shortSongGuid, "Come On, Come Over") { Lyrics ="Come on, come over"},
                    new Song(longSongGuid, "Another song") { Lyrics = "This song isn't real but it has more words" }
                });

            _statisticsServiceMock.Setup(x => x.GetStatistics(artistGuid)).ReturnsAsync(serviceResults).Verifiable();

            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.GetStatisticsForArtistAsync(artistGuid) as OkObjectResult;
            var data = (result.Value) as GetStatisticsForArtistResponseModel;

            // Assert
            _statisticsServiceMock.Verify();
            Assert.IsNotNull(data);
            Assert.AreEqual(artistGuid, data.Artist.Id);
            Assert.AreEqual(2, data.SongLengths.Length);
            Assert.AreEqual(4, data.SongLengths[0]);
            Assert.AreEqual(9, data.SongLengths[1]);
            Assert.AreEqual(shortSongGuid, data.ShortestSong.Id);
            Assert.AreEqual(longSongGuid, data.LongestSong.Id);
            Assert.AreEqual(6.25d, data.Variance);
            Assert.AreEqual(6.5d, data.AverageWordsPerSong);
            Assert.AreEqual(2.5d, data.StandardDeviation);
        }

        [Test]
        public async Task GetStatisticsForArtistAsync_ReturnsOk()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var serviceResults = new ArtistStatistics(
                new Artist(artistGuid, "Jaco Pastorius", "Person", "Bass player extraordinaire"),
                new Song[]
                {
                    new Song(Guid.NewGuid(),"Come On, Come Over") { Lyrics ="Come on, come over"}
                });

            _statisticsServiceMock.Setup(x => x.GetStatistics(artistGuid)).ReturnsAsync(serviceResults);


            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.GetStatisticsForArtistAsync(artistGuid) as OkObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public async Task GetStatisticsForArtistAsync_Returns500ForServiceException()
        {
            // Setup
            var artistGuid = Guid.NewGuid();

            _statisticsServiceMock.Setup(x => x.GetStatistics(artistGuid)).ThrowsAsync(new ThirdPartyServiceException("Problems!", new Exception()));

            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.GetStatisticsForArtistAsync(artistGuid) as StatusCodeResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        [Test]
        public async Task GetStatisticsForArtistAsync_ReturnsNotFoundForInvalidId()
        {
            // Setup
            var artistGuid = Guid.NewGuid();

            _statisticsServiceMock.Setup(x => x.GetStatistics(artistGuid)).ThrowsAsync(new ArtistNotFoundException(artistGuid));

            // Test
            var controller = new LyricsController(_artistServiceMock.Object, _lyricsServiceMock.Object, _statisticsServiceMock.Object);
            var result = await controller.GetStatisticsForArtistAsync(artistGuid) as StatusCodeResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        #endregion
    }
}