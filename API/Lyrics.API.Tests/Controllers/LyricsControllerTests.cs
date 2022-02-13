using Lyrics.API.Controllers;
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

        #region SearchByName

        [Test]
        public async Task Get_ReturnsDataRetrievedFromService()
        {
            // Setup
            var serviceResults = new List<Artist>
            {
                new Artist(Guid.NewGuid(),"Henge","band","Really good live"),
                new Artist(Guid.NewGuid(),"Henge","person", "Not even real")
            };
            _artistServiceMock.Setup(x => x.FindArtistsByNameAsync("Henge")).ReturnsAsync(serviceResults).Verifiable();

            // Test
            var controller = new LyricsController(_artistServiceMock.Object);
            var result = await controller.SearchByName("Henge") as OkObjectResult;

            // Assert
            _artistServiceMock.Verify();
            Assert.That(result.Value, NUnit.DeepObjectCompare.Is.DeepEqualTo(serviceResults));
        }

        [Test]
        public async Task Get_ReturnsOk()
        {
            // Setup
            var serviceResults = new List<Artist>
            {
                new Artist(Guid.NewGuid(),"Henge","band","Really good live"),
                new Artist(Guid.NewGuid(),"Henge","person", "Not even real")
            };
            _artistServiceMock.Setup(x => x.FindArtistsByNameAsync("Henge")).ReturnsAsync(serviceResults);

            // Test
            var controller = new LyricsController(_artistServiceMock.Object);
            var result = await controller.SearchByName("Henge") as OkObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Test]
        public async Task Get_Returns500ForServiceException()
        {
            // Setup
            var serviceResults = new List<Artist>
            {
                new Artist(Guid.NewGuid(),"Henge","band","Really good live"),
                new Artist(Guid.NewGuid(),"Henge","person", "Not even real")
            };
            _artistServiceMock.Setup(x => x.FindArtistsByNameAsync("Henge")).ThrowsAsync(new ThirdPartyServiceException("Rate limited!", new Exception("Too many requests")));

            // Test
            var controller = new LyricsController(_artistServiceMock.Object);
            var result = await controller.SearchByName("Henge") as StatusCodeResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
        }

        #endregion
    }
}