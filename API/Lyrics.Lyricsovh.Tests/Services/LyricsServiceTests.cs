using Lyrics.Common.Exceptions;
using Lyrics.Lyricsovh.Models;
using Lyrics.Lyricsovh.Services;
using Lyrics.Lyricsovh.Tests.Helpers;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lyrics.Lyricsovh.Tests.Services
{
    public class LyricsServiceTests
    {

        #region GetLyricsForSongAsync
        #endregion
        [Test]
        public async Task GetLyricsForSongAsync_ReturnsLyricsFromAPI()
        {
            // Setup
            var lyrics = new GetLyricsResponse { Lyrics = "The sun is in my eyes, whoo!\r\nWoke up and felt the vibe, whoo!\r\nNo matter how hard they try, whoo!\r\nWe never gonna die" };
            var mockedResponse = MockHttpMessageHandler.MockResponse(lyrics, HttpStatusCode.OK);

            var httpClient = new HttpClient(mockedResponse.Object);
            httpClient.BaseAddress = new Uri("https://localhost:3000");

            // Test
            var service = new LyricsService(httpClient);
            var result = await service.GetLyricsForSongAsync("Kanye", "Famous");

            // Assert
            Assert.AreEqual(lyrics.Lyrics, result);
        }

        [Test]
        public void GetLyricsForSongAsync_HandlesThirdPartyErrors()
        {
            // Setup
            var mockedResponse = MockHttpMessageHandler.MockResponse("Oh no, an error!", HttpStatusCode.InternalServerError);

            var httpClient = new HttpClient(mockedResponse.Object);
            httpClient.BaseAddress = new Uri("https://localhost:3000");

            // Test
            var service = new LyricsService(httpClient);

            // Assert
            Assert.ThrowsAsync<ThirdPartyServiceException>(async () => await service.GetLyricsForSongAsync("Kanye", "Famous"));
        }

        [Test]
        public async Task GetLyricsForSongAsync_HandlesNoLyricsFound()
        {
            // Setup
            var mockedResponse = MockHttpMessageHandler.MockResponse(JsonSerializer.Serialize("{\"error\":\"No lyrics found\"}"), HttpStatusCode.NotFound);

            var httpClient = new HttpClient(mockedResponse.Object);
            httpClient.BaseAddress = new Uri("https://localhost:3000");

            // Test
            var service = new LyricsService(httpClient);
            var result = await service.GetLyricsForSongAsync("Kanye", "Something that isnt in his catalogue");

            // Assert
            Assert.AreEqual(string.Empty, result);
        }
    }
}