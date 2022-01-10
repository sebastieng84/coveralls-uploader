using System.Linq;
using coveralls_uploader.JobProviders;
using coveralls_uploader.Utilities;
using Moq;
using NUnit.Framework;

namespace Tests.JobProviders;

public class JenkinsJobProviderTests
{
    private JenkinsJobProvider _sut;
    private Mock<IEnvironmentWrapper> _environmentWrapperMock;

    [SetUp]
    public void Setup()
    {
        _environmentWrapperMock = new Mock<IEnvironmentWrapper>();
        _sut = new JenkinsJobProvider(_environmentWrapperMock.Object);
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
            "COVERALLS_TOKEN",
            "BUILD_NUMBER", 
            "GIT_COMMIT",
            "GIT_AUTHOR_EMAIL",
            "GIT_AUTHOR_NAME", 
            "GIT_COMMITTER_EMAIL",
            "GIT_COMMITTER_NAME",
            "GIT_BRANCH", 
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
            Assert.AreEqual("COVERALLS_TOKEN_VALUE", job.RepositoryToken);
            Assert.AreEqual("BUILD_NUMBER_VALUE", job.ServiceNumber);
            Assert.AreEqual("COVERALLS_PULL_REQUEST_NUMBER_VALUE", job.ServicePullRequest);
            Assert.AreEqual("GIT_COMMIT_VALUE", job.CommitSha);
            Assert.AreEqual("GIT_COMMIT_VALUE", job.GitInformation.Head.Id);
            Assert.AreEqual("GIT_AUTHOR_EMAIL_VALUE", job.GitInformation.Head.AuthorEmail);
            Assert.AreEqual("GIT_AUTHOR_NAME_VALUE", job.GitInformation.Head.AuthorName);
            Assert.AreEqual("GIT_COMMITTER_EMAIL_VALUE", job.GitInformation.Head.CommitterEmail);
            Assert.AreEqual("GIT_COMMITTER_NAME_VALUE", job.GitInformation.Head.CommitterName);
            Assert.AreEqual("GIT_BRANCH_VALUE", job.GitInformation.Branch);
        });
    }
    
    [TestCase("https://github.com/user/test.git")]
    [TestCase("git@github.com:user/test.git")]
    public void WhenILoad_WithAValidGitUrl_TheItCreatesAValidRemote(string gitUrl)
    {
        // Arrange
        const string repositoryName = "test";

        _environmentWrapperMock
            .Setup(wrapper => wrapper.GetEnvironmentVariable("GIT_URL"))
            .Returns(gitUrl);
        
        // Act
        var job = _sut.Load();
        
        // Assert
        Assert.AreEqual(repositoryName, job.GitInformation.Remotes.First().Name);
        Assert.AreEqual(gitUrl, job.GitInformation.Remotes.First().Url);
    }
    
    [TestCase("https://github.com/user/test")]
    [TestCase("user.test.git")]
    public void WhenILoad_WithAnInvalidGitUrl_TheItDoesNotCreateARemote(string gitUrl)
    {
        // Arrange
        _environmentWrapperMock
            .Setup(wrapper => wrapper.GetEnvironmentVariable("GIT_URL"))
            .Returns(gitUrl);
        
        // Act
        var job = _sut.Load();
        
        // Assert
        Assert.IsEmpty(job.GitInformation.Remotes);
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
            Assert.IsNull(job.GitInformation.Head.Id);
            Assert.IsNull(job.GitInformation.Head.AuthorEmail);
            Assert.IsNull(job.GitInformation.Head.AuthorName);
            Assert.IsNull(job.GitInformation.Head.CommitterEmail);
            Assert.IsNull(job.GitInformation.Head.CommitterName);
            Assert.IsNull(job.GitInformation.Head.Message);
            Assert.IsNull(job.GitInformation.Branch);
        });
    }
}