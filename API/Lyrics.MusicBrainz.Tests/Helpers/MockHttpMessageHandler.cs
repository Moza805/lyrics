using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Lyrics.MusicBrainz.Tests.Helpers
{
    public static class MockHttpMessageHandler
    {
        public static Mock<HttpMessageHandler> MockResponse(object response, HttpStatusCode statusCode)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected().Setup<Task<HttpResponseMessage>>(
                 "SendAsync",
               ItExpr.IsAny<HttpRequestMessage>(),
                 ItExpr.IsAny<CancellationToken>())
                 .ReturnsAsync(
                     new HttpResponseMessage()
                     {
                         StatusCode = statusCode,
                         Content = new StringContent(JsonSerializer.Serialize(response, new JsonSerializerOptions(JsonSerializerDefaults.Web)), Encoding.UTF8, "application/json"),
                     }
                 );

            return handlerMock;
        }
    }
}
