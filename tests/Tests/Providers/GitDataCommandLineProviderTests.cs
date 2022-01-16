using System;
using System.Collections.Generic;
using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Providers;
using coveralls_uploader.Utilities;
using Moq;
using NUnit.Framework;
using Serilog;

namespace Tests.Providers;

public class GitDataCommandLineProviderTests
{
    private GitDataCommandLineProvider _sut; 
    private Mock<ILogger> _loggerMock;
    private Mock<CommandLineHelper> _commandLineHelperMock;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger>();
        _commandLineHelperMock = new Mock<CommandLineHelper>();
        
        _sut = new GitDataCommandLineProvider(_loggerMock.Object, _commandLineHelperMock.Object);
    }

    private static IEnumerable<TestCaseData> GetRemotesTestCases()
    {
        yield return new TestCaseData(
            "origin  https://github.com/user/test.git (fetch)\norigin  https://github.com/user/test.git (push)\n",
            new List<Remote>
            {
                new("origin", "https://github.com/user/test.git")
            });
        yield return new TestCaseData(
            "test1  url1 (fetch)\n test2  url2 (push)\n test3  url3 (fetch)\ntest4  url4 (push)\n",
            new List<Remote>
            {
                new("test1", "url1"), 
                new("test3", "url3")
            });
        yield return new TestCaseData(new Guid().ToString(), new List<Remote>());
        yield return new TestCaseData(string.Empty, new List<Remote>());
    }

    [Test]
    public void WhenIGetBranch_AndCommandLineFails_ThenNull()
    {
        // Arrange
        var output = string.Empty;
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(false);
        
        // Act
        var branch = _sut.GetBranch();
        
        // Assert
        Assert.IsNull(branch);
    }
    
    [TestCase("test", "test")]
    [TestCase("  test  ", "test")]
    [TestCase("", null)]
    public void WhenIGetBranch_ReturnsTheCurrentBranch(
        string output,
        string expectedBranch)
    {
        // Arrange
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(true);
        
        // Act
        var branch = _sut.GetBranch();
        
        // Assert
        Assert.AreEqual(expectedBranch, branch);
    }
    
    [Test]
    public void WhenIGetRemotes_AndCommandLineFails_ThenNull()
    {
        // Arrange
        var output = string.Empty;
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(false);
        
        // Act
        var remotes = _sut.GetRemotes();
        
        // Assert
        Assert.IsEmpty(remotes);
    }
    
    [TestCaseSource(nameof(GetRemotesTestCases))]
    public void WhenIGetRemotes_ThenItReturnsThePushRemotes(
        string output,
        IList<Remote> expectedRemotes)
    {
        // Arrange
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(true);
        
        // Act
        var remotes = _sut.GetRemotes();
        
        // Assert
        CollectionAssert.AreEqual(expectedRemotes, remotes);
    }
    
    [Test]
    public void WhenIGetHead_AndCommandLineFails_ThenNull()
    {
        // Arrange
        var output = string.Empty;
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(false);
        
        // Act
        var head = _sut.GetHead(null);
        
        // Assert
        Assert.IsNull(head);
    }
}