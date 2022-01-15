using System;
using System.Diagnostics;
using coveralls_uploader.Utilities;
using NUnit.Framework;

namespace Tests.Utilities;

public class ProcessFactoryTests
{
    private ProcessFactory _sut;

    [SetUp]
    public void Setup()
    {
        _sut = new ProcessFactory();
    }
    
    [Test]
    public void WhenICreate_ThenItSetsTheProcessStartInfo()
    {
        // Arrange
        var processStartInfo = new ProcessStartInfo();
        
        // Act
        var process = _sut.Create(processStartInfo);
        
        // Assert
        Assert.AreEqual(processStartInfo, process.StartInfo);
    }
    
    [Test]
    public void WhenICreate_WithNullStartInfo_ThenItThrows()
    {
        Assert.Throws<ArgumentNullException>(() => _sut.Create(null));
    }
}