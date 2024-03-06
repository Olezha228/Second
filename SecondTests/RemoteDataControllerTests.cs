using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Second.Controllers;

namespace YourNamespace.Tests
{
    public class RemoteDataControllerTests
    {
        private Mock<HttpClient> _mockHttpClient;
        private RemoteDataController _controller;

        public RemoteDataControllerTests()
        {
            _mockHttpClient = new Mock<HttpClient>();
            _controller = new RemoteDataController(_mockHttpClient.Object);
        }

        [Fact]
        public async Task Get_ReturnsSuccessResult_WhenDataIsRetrievedSuccessfully()
        {
            // Arrange
            var expectedData = "{\"key\":\"value\"}";
            _mockHttpClient.Setup(httpClient => httpClient.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedData)
                }));

            // Act
            var result = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualData = JsonConvert.DeserializeObject<string>(okResult.Value.ToString());
            Assert.Equal(expectedData, actualData);
        }

        [Fact]
        public async Task Get_ReturnsErrorResult_WhenDataCannotBeRetrieved()
        {
            // Arrange
            _mockHttpClient.Setup(httpClient => httpClient.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                }));

            // Act
            var result = await _controller.Get();

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCodeResult.StatusCode);
        }
    }
}