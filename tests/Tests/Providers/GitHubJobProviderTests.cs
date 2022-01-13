using coveralls_uploader.Providers;
using coveralls_uploader.Utilities;
using Moq;
using NUnit.Framework;

namespace Tests.Providers;

public class GitHubJobProviderTests
{
    private GitHubJobProvider _sut;
    private Mock<IEnvironmentWrapper> _environmentWrapperMock;

    [SetUp]
    public void Setup()
    {
        _environmentWrapperMock = new Mock<IEnvironmentWrapper>();
        _sut = new GitHubJobProvider(_environmentWrapperMock.Object);
    }

    [Test]
    public void WhenILoad_ThenTheJobHasTheRightServiceName()
    {
        // Arrange
        // Act
        var job = _sut.Load();
        
        // Assert
        Assert.AreEqual(_sut.ServiceName, job.ServiceName);
    }
    
    [Test]
    public void WhenILoad_ThenItUsesRightEnvironmentVariables()
    {
        // Arrange
        var environmentVariables = new[]
        {
            "GITHUB_TOKEN",
            "GITHUB_RUN_NUMBER", 
            "GITHUB_RUN_ID",
            "GITHUB_SHA",
            "GITHUB_REF", 
            "COVERALLS_PULL_REQUEST_NUMBER"
        };

        foreach (var environmentVariable in environmentVariables)
        {
            _environmentWrapperMock
                .Setup(wrapper => wrapper.GetEnvironmentVariable(environmentVariable))
                .Returns(environmentVariable + "_VALUE");
        }
        
        // Act
        var job = _sut.Load();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.AreEqual("GITHUB_TOKEN_VALUE", job.RepositoryToken);
            Assert.AreEqual("GITHUB_RUN_NUMBER_VALUE", job.ServiceNumber);
            Assert.AreEqual("GITHUB_RUN_ID_VALUE", job.ServiceJobId);
            Assert.AreEqual("COVERALLS_PULL_REQUEST_NUMBER_VALUE", job.ServicePullRequest);
            Assert.AreEqual("GITHUB_SHA_VALUE", job.CommitSha);
            Assert.AreEqual("GITHUB_REF_VALUE", job.Git.Branch);
        });
    }
    
    [Test]
    public void WhenILoad_AndEnvironmentVariablesAreNotSet_TheItReturnsNullValues()
    {
        // Arrange
        // Act
        var job = _sut.Load();
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.IsNull(job.RepositoryToken);
            Assert.IsNull(job.ServiceNumber);
            Assert.IsNull(job.ServiceJobId);
            Assert.IsNull(job.CommitSha);
            Assert.IsNull(job.ServicePullRequest);
        });
    }
}