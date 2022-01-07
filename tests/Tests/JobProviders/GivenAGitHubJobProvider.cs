using coveralls_uploader.JobProviders;
using coveralls_uploader.Utilities;
using Moq;
using NUnit.Framework;

namespace Tests.JobProviders;

public class GivenAGitHubJobProvider
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
        // Given
        // When
        var job = _sut.Load();
        
        // Then
        Assert.AreEqual(_sut.ServiceName, job.ServiceName);
    }
    
    [Test]
    public void WhenILoad_ThenItUsesRightEnvironmentVariables()
    {
        // Given
        var environmentVariables = new[]
        {
            "GITHUB_TOKEN", "GITHUB_RUN_NUMBER", "GITHUB_RUN_ID", "GITHUB_SHA", "GIT_COMMIT_AUTHOR_EMAIL",
            "GIT_COMMIT_AUTHOR_NAME", "GIT_COMMIT_COMMITTER_EMAIL", "GIT_COMMIT_COMMITTER_NAME",
            "GIT_COMMIT_MESSAGE_BODY", "GITHUB_REF"
        };

        foreach (var environmentVariable in environmentVariables)
        {
            _environmentWrapperMock
                .Setup(wrapper => wrapper.GetEnvironmentVariable(environmentVariable))
                .Returns(environmentVariable + "_VALUE");
        }
        
        // When
        var job = _sut.Load();
        
        // Then
        Assert.Multiple(() =>
        {
            Assert.AreEqual("GITHUB_TOKEN_VALUE", job.RepositoryToken);
            Assert.AreEqual("GITHUB_RUN_NUMBER_VALUE", job.ServiceNumber);
            Assert.AreEqual("GITHUB_RUN_ID_VALUE", job.ServiceJobId);
            Assert.AreEqual("GITHUB_SHA_VALUE", job.CommitSha);
            Assert.AreEqual("GITHUB_SHA_VALUE", job.GitInformation.Head.Id);
            Assert.AreEqual("GIT_COMMIT_AUTHOR_EMAIL_VALUE", job.GitInformation.Head.AuthorEmail);
            Assert.AreEqual("GIT_COMMIT_AUTHOR_NAME_VALUE", job.GitInformation.Head.AuthorName);
            Assert.AreEqual("GIT_COMMIT_COMMITTER_EMAIL_VALUE", job.GitInformation.Head.CommitterEmail);
            Assert.AreEqual("GIT_COMMIT_COMMITTER_NAME_VALUE", job.GitInformation.Head.CommitterName);
            Assert.AreEqual("GIT_COMMIT_MESSAGE_BODY_VALUE", job.GitInformation.Head.Message);
            Assert.AreEqual("GITHUB_REF_VALUE", job.GitInformation.Branch);
        });
    }
    
    [Test]
    public void WhenILoad_AndEnvironmentVariablesAreNotSet_TheItReturnsNullValues()
    {
        // Given
        // When
        var job = _sut.Load();
        
        // Then
        Assert.Multiple(() =>
        {
            Assert.IsNull(job.RepositoryToken);
            Assert.IsNull(job.ServiceNumber);
            Assert.IsNull(job.ServiceJobId);
            Assert.IsNull(job.CommitSha);
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