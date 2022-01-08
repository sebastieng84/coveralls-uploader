using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using coveralls_uploader.Commands;
using coveralls_uploader.Models;
using coveralls_uploader.Services;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Tests.Commands;

public class UploadCommandHandlerTests
{
    private UploadCommandHandler _sut;

    private Mock<IHost> _hostMock;
    private Mock<MainService> _mainServiceMock;
    private Mock<ILogger> _loggerMock;

    [SetUp]
    public void Setup()
    {
        _hostMock = new Mock<IHost>();
        _mainServiceMock = new Mock<MainService>();
        _loggerMock = new Mock<ILogger>();
        
        _sut = new UploadCommandHandler(_hostMock.Object);

        _hostMock
            .Setup(host => host.Services.GetService(typeof(MainService)))
            .Returns(_mainServiceMock.Object);
        _hostMock
            .Setup(host => host.Services.GetService(typeof(ILogger)))
            .Returns(_loggerMock.Object);
    }

    public static IEnumerable<TestCaseData> CommandOptionsTestCases()
    {
        yield return new TestCaseData(new FileInfo("/test.txt"), true, string.Empty);
        yield return new TestCaseData(new FileInfo("test.txt"), false, Guid.NewGuid().ToString());
    }

    [TestCase(true, LogEventLevel.Verbose)]
    [TestCase(false, LogEventLevel.Debug)]
    public async Task WhenIInvokeAsync_WithVerboseOption_ThenItChangesTheMinimumLevelToVerbose(
        [Values] bool verbose,
        LogEventLevel expectedLogEventLevel)

    {
        // Arrange
        _sut.Verbose = verbose;
        
        const LogEventLevel defaultLogEventLevel = LogEventLevel.Debug;
        var loggingLevelSwitch = new LoggingLevelSwitch(defaultLogEventLevel);

        _hostMock
            .Setup(host => host.Services.GetService(typeof(LoggingLevelSwitch)))
            .Returns(loggingLevelSwitch);

        // Act
        await _sut.InvokeAsync(null);

        // Assert
        Assert.AreEqual(expectedLogEventLevel, loggingLevelSwitch.MinimumLevel);
    }
    
    [Test]
    public async Task WhenIInvokeAsync_AndAnExceptionIsThrown_ThenItReturnsAnErrorCode()
    {
        // Arrange
        const int expectedExitCode = -1;
        
        _mainServiceMock
            .Setup(service => service.RunAsync(It.IsAny<CommandOptions>()))
            .ThrowsAsync(new ArgumentOutOfRangeException());

        // Act
        var exitCode = await _sut.InvokeAsync(null);

        // Assert
        Assert.AreEqual(expectedExitCode, exitCode);
    }
    
    [Test]
    public async Task WhenIInvokeAsync_AndAnExceptionIsThrown_ThenItLogsTheException()
    {
        // Arrange
        var exception = new ArgumentOutOfRangeException();
        
        _mainServiceMock
            .Setup(service => service.RunAsync(It.IsAny<CommandOptions>()))
            .ThrowsAsync(exception);

        // Act
        await _sut.InvokeAsync(null);

        // Assert
        _loggerMock.Verify(logger => logger.Error(exception, It.IsAny<string>()), Times.Once);
    }
    
    [Test]
    public async Task WhenIInvokeAsync_ThenItReturnsZero()
    {
        // Arrange
        const int expectedExitCode = 0;
        
        // Act
        var exitCode = await _sut.InvokeAsync(null);

        // Assert
        Assert.AreEqual(expectedExitCode, exitCode);
    }
    
    [TestCaseSource(nameof(CommandOptionsTestCases))]
    public async Task WhenIInvokeAsync_ThenItCreatesAValidCommandOptions(
        FileInfo input,
        bool source,
        string token)
    {
        // Arrange
        _sut.Input = input;
        _sut.Source = source;
        _sut.Token = token;
        
        // Act
        var exitCode = await _sut.InvokeAsync(null);

        // Assert
        _mainServiceMock.Verify(service => service.RunAsync(It.Is<CommandOptions>(options =>
            options.Source == source && 
            options.Input == input && 
            options.Token == token)));
    }
}