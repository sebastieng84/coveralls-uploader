using System.Linq;
using AutoFixture;
using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Providers;
using coveralls_uploader.Utilities;
using Moq;
using NUnit.Framework;
using Serilog;

namespace Tests.Providers;

public class GitDataCommandLineProviderTests
{
    private readonly Fixture _fixture = new();
    
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

    [Test]
    public void WhenIGetBranch_AndCommandLineFails_ThenNull()
    {
        // Arrange
        var output = string.Empty;
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(false);
        
        // Act
        var actual = _sut.GetBranch();
        
        // Assert
        Assert.IsNull(actual);
    }
    
    [TestCase("test", "test")]
    [TestCase("  test  ", "test")]
    public void WhenIGetBranch_ReturnsBranchTrimmed(
        string output,
        string expected)
    {
        // Arrange
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(true);
        
        // Act
        var actual = _sut.GetBranch();
        
        // Assert
        Assert.AreEqual(expected, actual);
    }
    
    [Test]
    public void WhenIGetBranch_WithNoCurrentBranch_ReturnsNull()
    {
        // Arrange
        var output = string.Empty;
        
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(true);
        
        // Act
        var actual = _sut.GetBranch();
        
        // Assert
        Assert.IsNull(actual);
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
        var actual = _sut.GetRemotes();
        
        // Assert
        Assert.IsEmpty(actual);
    }
    
    [Test]
    public void WhenIGetRemotes_ThenItReturnsThePushRemote()
    {
        // Arrange
        var expected = _fixture.Create<Remote>();
        var output =
            $"{expected.Name}  {expected.Url} (push)\n" +
            $"{expected.Name}  {expected.Url} (fetch)\n";
        
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(true);
        
        // Act
        var actual = _sut.GetRemotes();
        
        // Assert
        CollectionAssert.AreEqual(expected.InList(), actual);
    }
    
    [Test]
    public void WhenIGetRemotes_WithMultipleRemotes_ThenItReturnsThePushRemotes()
    {
        // Arrange
        var expected = _fixture.CreateMany<Remote>(2).ToList();

        var output =
            $"{expected[0].Name} {expected[0].Url} (push)\n" +
            $"{expected[0].Name} {expected[0].Url} (fetch)\n" +
            $"{expected[1].Name} {expected[1].Url} (push)\n" +
            $"{expected[1].Name} {expected[1].Url} (fetch)\n";
        
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(true);
        
        // Act
        var actual = _sut.GetRemotes();
        
        // Assert
        CollectionAssert.AreEqual(expected, actual);
    }
    
    [Test]
    public void WhenIGetRemotes_WithNoRemotes_ThenItReturnsAnEmptyList()
    {
        // Arrange
        var output = string.Empty;
        
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(true);
        
        // Act
        var actual = _sut.GetRemotes();
        
        // Assert
        CollectionAssert.IsEmpty(actual);
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
        var actual = _sut.GetHead(null);
        
        // Assert
        Assert.IsNull(actual);
    }
    
    [Test]
    public void WhenIGetHead_ThenItReturnsTheCommandOutputInformation()
    {
        // Arrange
        const char separator = '\n';
        var expected = _fixture.Create<Head>();

        var output =
            $"{expected.Id}{separator}{expected.AuthorName}{separator}{expected.AuthorEmail}" +
            $"{separator}{expected.CommitterName}{separator}{expected.CommitterEmail}{separator}" +
            $"{expected.Message}";
        
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(true);
        
        // Act
        var actual = _sut.GetHead(expected.Id);
        
        // Assert
        Assert.AreEqual(expected, actual);
    }
    
    [Test]
    public void WhenIGetHead_WithMissingInformation_ThenItDoesNotThrow()
    {
        // Arrange
        const char separator = '\n';
        var expected = _fixture.Build<Head>()
            .With(head => head.Message, string.Empty)
            .With(head => head.CommitterName, string.Empty)
            .With(head => head.CommitterEmail, string.Empty)
            .Create();

        var output =
            $"{expected.Id}{separator}{expected.AuthorName}{separator}{expected.AuthorEmail}" +
            $"{separator}{expected.CommitterName}{separator}{expected.CommitterEmail}{separator}" +
            $"{expected.Message}";
        
        _commandLineHelperMock
            .Setup(helper => helper.TryRun(It.IsAny<string>(), out output))
            .Returns(true);

        // Act
        Head actual = null;
        Assert.DoesNotThrow(() => actual = _sut.GetHead(expected.Id));
        
        // Assert
        Assert.AreEqual(expected, actual);
    }
    
    [Test]
    public void WhenILoad_ThenItGetTheBranchRemotesAndHead()
    {
        // Arrange
        const string commitSha = "commit";
        var output = string.Empty;

        // Act
        var actual = _sut.Load(commitSha);
        
        // Assert
        _commandLineHelperMock.Verify(helper => helper.TryRun(It.IsAny<string>(), out output), Times.Exactly(3));
    }
}