using coveralls_uploader.Providers;
using coveralls_uploader.Utilities.Wrappers;
using Moq;
using NUnit.Framework;

namespace Tests.Providers;

public class JenkinsJobProviderTests
{
    private JenkinsJobProvider _sut;
    private Mock<IEnvironment> _environmentWrapperMock;

    [SetUp]
    protected void Setup()
    {
        _environmentWrapperMock = new Mock<IEnvironment>();
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
            Assert.AreEqual("GIT_COMMIT_VALUE", job.Git.Head.Id);
            Assert.AreEqual("GIT_AUTHOR_EMAIL_VALUE", job.Git.Head.AuthorEmail);
            Assert.AreEqual("GIT_AUTHOR_NAME_VALUE", job.Git.Head.AuthorName);
            Assert.AreEqual("GIT_COMMITTER_EMAIL_VALUE", job.Git.Head.CommitterEmail);
            Assert.AreEqual("GIT_COMMITTER_NAME_VALUE", job.Git.Head.CommitterName);
            Assert.AreEqual("GIT_BRANCH_VALUE", job.Git.Branch);
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
            Assert.IsNull(job.Git.Head.Id);
            Assert.IsNull(job.Git.Head.AuthorEmail);
            Assert.IsNull(job.Git.Head.AuthorName);
            Assert.IsNull(job.Git.Head.CommitterEmail);
            Assert.IsNull(job.Git.Head.CommitterName);
            Assert.IsNull(job.Git.Head.Message);
            Assert.IsNull(job.Git.Branch);
        });
    }
    
    [TestCase("origin/dev", "dev")]
    [TestCase("origin/dev/tests", "dev/tests")]
    public void WhenILoad_AndGitBranchIsARemoteBranch_TheItReturnsTheLocalBranch(
        string branch, 
        string expectedBranch)
    {
        // Arrange
        _environmentWrapperMock
            .Setup(wrapper => wrapper.GetEnvironmentVariable("GIT_BRANCH"))
            .Returns(branch);
        
        // Act
        var job = _sut.Load();
        
        // Assert
        Assert.AreEqual(expectedBranch, job.Git.Branch);
    }
}