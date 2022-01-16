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

    private static IEnumerable<TestCaseData> GetBranchTestCases()
    {
        yield return new TestCaseData("* test\n  master\n  test2\n", "test");
        yield return new TestCaseData("  master\n* test\n  test2\n", "test");
        yield return new TestCaseData("  test\n  master\n  test2\n", null);
        yield return new TestCaseData(new Guid().ToString(), null);
        yield return new TestCaseData(string.Empty, null);
    }
    
    private static IEnumerable<TestCaseData> GetRemotesTestCases()
    {
        yield return new TestCaseData(
            "origin  https://github.com/user/test.git (fetch)\norigin  https://github.com/user/test.git (push)\n",
            new List<Remote> {new("origin", "https://github.com/user/test.git")});
        yield return new TestCaseData("", new List<Remote>());
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
    
    [TestCaseSource(nameof(GetBranchTestCases))]
    public void WhenIGetBranch_ThenItReturnsTheCurrentBranch(
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