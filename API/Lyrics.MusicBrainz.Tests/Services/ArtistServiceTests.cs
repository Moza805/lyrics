using Lyrics.Common.Exceptions;
using Lyrics.MusicBrainz.Constants;
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
        #region FindArtistsByNameAsync

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
            httpClient.BaseAddress = new Uri("https://localhost:3000");

            // Test
            var service = new ArtistService(httpClient);
            var result = (await service.FindArtistsByNameAsync("Bonobo")).ToList();

            // Assert
            Assert.AreEqual("Bonobo", result[0].Name);
            Assert.AreEqual("Bonobo", result[1].Name);
            Assert.AreEqual("Lofi electronic musician from Brighton", result[0].Disambiguation);
            Assert.AreEqual("Dutch breakbeat group", result[1].Disambiguation);
        }

        [Test]
        public void FindArtistsByNameAsync_HandlesThirdParty500()
        {
            var mockedResponse = MockHttpMessageHandler.MockResponse("Oh no, an error", HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(mockedResponse.Object);
            httpClient.BaseAddress = new Uri("https://localhost:3000");

            // Test
            var service = new ArtistService(httpClient);
            Assert.ThrowsAsync<ThirdPartyServiceException>(async () => await service.FindArtistsByNameAsync("Bonobo"));
        }

        #endregion

        #region GetSongsByArtistAsync

        public async Task GetSongsByArtistAsync_ReturnsData()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var musicBrainsResponse = new SongListResponse
            {
                Works = new List<Work>
                {
                    new Work { Id = Guid.NewGuid(), Title="Boot and Spleen", Disambiguation = "Lots of saxophone", Type=WorkConstants.WorkTypes.Song },
                    new Work { Id = Guid.NewGuid(), Title="Jillian", Disambiguation = "The yoga teacher", Type=WorkConstants.WorkTypes.Song }
                }
            };

            var mockedResponse = MockHttpMessageHandler.MockResponse(musicBrainsResponse, HttpStatusCode.OK);
            var httpClient = new HttpClient(mockedResponse.Object);
            httpClient.BaseAddress = new Uri("https://localhost:3000");

            // Test
            var service = new ArtistService(httpClient);
            var result = (await service.GetSongsByArtistAsync(artistGuid)).ToList();

            // Assert
            Assert.AreEqual("Boot and Spleen", result[0].Title);
            Assert.AreEqual("Jillian", result[1].Title);
            Assert.AreEqual(musicBrainsResponse.Works[0].Id, result[0].Id);
            Assert.AreEqual(musicBrainsResponse.Works[1].Id, result[1].Id);
        }

        [Test]
        public async Task GetSongsByArtistAsync_FiltersOutAlbums()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var musicBrainsResponse = new SongListResponse
            {
                Works = new List<Work>
                {
                    new Work { Id = Guid.NewGuid(), Title="Boot and Spleen", Disambiguation = "Lots of saxophone", Type=WorkConstants.WorkTypes.Song },
                    new Work { Id = Guid.NewGuid(), Title="Jillian", Disambiguation = "The yoga teacher" , Type=WorkConstants.WorkTypes.Album }
                }
            };

            var mockedResponse = MockHttpMessageHandler.MockResponse(musicBrainsResponse, HttpStatusCode.OK);
            var httpClient = new HttpClient(mockedResponse.Object);
            httpClient.BaseAddress = new Uri("https://localhost:3000");

            // Test
            var service = new ArtistService(httpClient);
            var result = (await service.GetSongsByArtistAsync(artistGuid)).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.AreEqual("Boot and Spleen", result[0].Title);
            Assert.AreEqual(musicBrainsResponse.Works[0].Id, result[0].Id);
        }

        [Test]
        public void GetSongsByArtistAsync_HandlesThirdParty500()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var mockedResponse = MockHttpMessageHandler.MockResponse("Oh no, an error", HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(mockedResponse.Object);
            httpClient.BaseAddress = new Uri("https://localhost:3000");

            // Test
            var service = new ArtistService(httpClient);

            // Assert
            Assert.ThrowsAsync<ThirdPartyServiceException>(async () => await service.GetSongsByArtistAsync(artistGuid));
        }

        [Test]
        public async Task GetSongsByArtistAsync_HandlesInvalidId()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var mockedResponse = MockHttpMessageHandler.MockResponse("Unrecognised Id", HttpStatusCode.NotFound);
            var httpClient = new HttpClient(mockedResponse.Object);
            httpClient.BaseAddress = new Uri("https://localhost:3000");

            // Test
            var service = new ArtistService(httpClient);
            var result = await service.GetSongsByArtistAsync(artistGuid);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        #endregion

        #region GetArtistByIdAsync

        public async Task GetArtistByIdAsync_ReturnsData()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var musicBrainsResponse = new Artist
            {
                Id = Guid.NewGuid(),
                Name = "Daft Punk",
                Disambiguation = "French electronic dance",
                Type = ArtistConstants.ArtistTypes.Group
            };

            var mockedResponse = MockHttpMessageHandler.MockResponse(musicBrainsResponse, HttpStatusCode.OK);
            var httpClient = new HttpClient(mockedResponse.Object);
            httpClient.BaseAddress = new Uri("https://localhost:3000");

            // Test
            var service = new ArtistService(httpClient);
            var result = await service.GetArtistByIdAsync(artistGuid);

            // Assert
            Assert.AreEqual(musicBrainsResponse.Id, result.Id);
            Assert.AreEqual(musicBrainsResponse.Name, result.Name);
            Assert.AreEqual(musicBrainsResponse.Disambiguation, result.Disambiguation);
            Assert.AreEqual(musicBrainsResponse.Type, result.Type);
        }

        [Test]
        public void GetArtistByIdAsync_HandlesThirdParty500()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var mockedResponse = MockHttpMessageHandler.MockResponse("Oh no, an error", HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(mockedResponse.Object);
            httpClient.BaseAddress = new Uri("https://localhost:3000");

            // Test
            var service = new ArtistService(httpClient);

            // Assert
            Assert.ThrowsAsync<ThirdPartyServiceException>(async () => await service.GetArtistByIdAsync(artistGuid));
        }

        [Test]
        public void GetArtistByIdAsync_HandlesInvalidId()
        {
            // Setup
            var artistGuid = Guid.NewGuid();
            var mockedResponse = MockHttpMessageHandler.MockResponse("Unrecognised Id", HttpStatusCode.NotFound);
            var httpClient = new HttpClient(mockedResponse.Object);
            httpClient.BaseAddress = new Uri("https://localhost:3000");

            // Test
            var service = new ArtistService(httpClient);

            // Assert
            Assert.ThrowsAsync<ArtistNotFoundException>(async () => await service.GetArtistByIdAsync(artistGuid));
        }

        #endregion
    }
}