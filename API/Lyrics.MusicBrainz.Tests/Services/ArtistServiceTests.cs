using Lyrics.MusicBrainz.Models;
using Lyrics.MusicBrainz.Services;
using Lyrics.MusicBrainz.Tests.Helpers;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lyrics.MusicBrainz.Tests.Services
{
    public class ArtistServiceTests
    {
        [Test]
        public async Task FindArtistsByNameAsync_ReturnsData()
        {
            // Setup
            var musicBrainsResponse = new ArtistSearchResponse
            {
                Artists = new List<Artist>
                {
                    new Artist { Id = Guid.NewGuid(), Name="Bonobo", Disambiguation = "Lofi electronic musician from Brighton" },
                    new Artist { Id = Guid.NewGuid(), Name="Bonobo", Disambiguation = "Dutch breakbeat group" }
                }
            };

            var mockedResponse = MockHttpMessageHandler.MockResponse(musicBrainsResponse, HttpStatusCode.OK);
            var httpClient = new HttpClient(mockedResponse.Object);

            // Test
            var service = new ArtistService("test-name", new System.Version(1, 0, 0), "test@email.test", httpClient);
            var result = (await service.FindArtistsByNameAsync("Bonobo")).ToList();

            // Assert
            Assert.AreEqual("Bonobo", result[0].Name);
            Assert.AreEqual("Bonobo", result[1].Name);
            Assert.AreEqual("Lofi electronic musician from Brighton", result[0].Disambiguation);
            Assert.AreEqual("Dutch breakbeat group", result[1].Disambiguation);
        }
    }
}