using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using coveralls_uploader.Providers;
using coveralls_uploader.Utilities;
using coveralls_uploader.Utilities.Wrappers;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;
using Serilog;

namespace Tests.Providers;

public class GitDataProviderFactoryTests
{
    private GitDataProviderFactory _sut;
    private Mock<IHost> _hostMock;
    private Mock<IRuntimeInformation> _runtimeInformationMock;

    [SetUp]
    protected void Setup()
    {
        _hostMock = new Mock<IHost>();
        _runtimeInformationMock = new Mock<IRuntimeInformation>();
        
        _sut = new GitDataProviderFactory(_hostMock.Object, _runtimeInformationMock.Object);
    }

    private static IEnumerable<TestCaseData> OSPlatformTestCases()
    {
        yield return new TestCaseData(OSPlatform.Linux, typeof(LinuxGitDataCommandLineProvider));
        yield return new TestCaseData(OSPlatform.Windows, typeof(WindowsGitDataCommandLineProvider));
        yield return new TestCaseData(OSPlatform.OSX, typeof(OsxGitDataCommandLineProvider));
    }
    
    [TestCaseSource(nameof(OSPlatformTestCases))]
    public void WhenICreate_ItReturnsAnInstanceBasedOnTheOSPlatform(
        OSPlatform osPlatform, 
        Type expectedType)
    {
        // Arrange
        var processFactoryMock = new Mock<ProcessFactory>();
        var loggerMock = new Mock<ILogger>();
        
        _hostMock
            .Setup(host => host.Services.GetService(expectedType))
            .Returns(Activator.CreateInstance(expectedType, loggerMock.Object, processFactoryMock.Object));
        
        _runtimeInformationMock
            .Setup(info => info.IsOSPlatform(osPlatform))
            .Returns(true);
        
        // Act
        var gitDataProvider = _sut.Create();
        
        // Assert
        Assert.IsInstanceOf(expectedType, gitDataProvider);
        _hostMock.Verify(host => host.Services.GetService(expectedType), Times.Once);
    }
    
    [Test]
    public void WhenICreate_WithAnUnsupportedPlatform_ItThrows()
    {
        Assert.Throws<PlatformNotSupportedException>(() => _sut.Create());
    }
}