using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using static Moq.It;

namespace Tests.Services;

public class GivenACoverallsService
{
    private CoverallsService _sut;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new();
    private readonly Mock<ILogger> _loggerMock = new();
    
    [SetUp]
    public void Setup()
    {
        _sut = new CoverallsService(new HttpClient(_httpMessageHandlerMock.Object), _loggerMock.Object);
        
        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage());
    }

    [Test]
    public async Task WhenIUploadAsync_ThenLogDebugIsCalled()
    {
        // Arrange
        var job = new Job();
        
        // Act
        await _sut.UploadAsync(job);

        // Assert
        _loggerMock.Verify(logger => logger.Log(
                LogLevel.Debug,
                IsAny<EventId>(),
                IsAny<IsAnyType>(),
                IsAny<Exception?>(),
                IsAny<Func<IsAnyType, Exception?, string>>()),
            Times.Exactly(2));
    }
}