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
            "CHANGE_ID"
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
            Assert.AreEqual("BUILD_NUMBER_VALUE", job.ServiceJobId);
            Assert.AreEqual("CHANGE_ID_VALUE", job.ServicePullRequest);
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
    public void WhenILoad_AndEnvironmentVariablesAreNotSet_ThenItReturnsNullValues()
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
    public void WhenILoad_AndGitBranchIsARemoteBranch_ThenItReturnsTheLocalBranch(
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
    
    [Test]
    public void WhenILoad_AndVariableGitBranchIsNull_ThenItReturnsChangeBranch()
    {
        // Arrange
        const string expected = "branch";
        
        _environmentWrapperMock
            .Setup(wrapper => wrapper.GetEnvironmentVariable("GIT_BRANCH"))
            .Returns((string) null);
        _environmentWrapperMock
            .Setup(wrapper => wrapper.GetEnvironmentVariable("CHANGE_BRANCH"))
            .Returns(expected);
        
        // Act
        var job = _sut.Load();
        
        // Assert
        Assert.AreEqual(expected, job.Git.Branch);
    }
    
    [Test]
    public void WhenILoad_AndVariablesGitBranchAndChangeBranchAreNull_ThenItReturnsBranchNameAsBranch()
    {
        // Arrange
        const string expected = "branch";
        
        _environmentWrapperMock
            .Setup(wrapper => wrapper.GetEnvironmentVariable("GIT_BRANCH"))
            .Returns((string) null);
        _environmentWrapperMock
            .Setup(wrapper => wrapper.GetEnvironmentVariable("CHANGE_BRANCH"))
            .Returns((string) null);
        _environmentWrapperMock
            .Setup(wrapper => wrapper.GetEnvironmentVariable("BRANCH_NAME"))
            .Returns(expected);
        
        // Act
        var job = _sut.Load();
        
        // Assert
        Assert.AreEqual(expected, job.Git.Branch);
    }
    
    [Test]
    public void WhenILoad_AndVariableChangeIdIsNull_ThenItReturnsGhprbPullIdAsServicePullRequest()
    {
        // Arrange
        const string expected = "pull_request";
        
        _environmentWrapperMock
            .Setup(wrapper => wrapper.GetEnvironmentVariable("CHANGE_ID"))
            .Returns((string) null);
        _environmentWrapperMock
            .Setup(wrapper => wrapper.GetEnvironmentVariable("ghprbPullId"))
            .Returns(expected);
        
        // Act
        var job = _sut.Load();
        
        // Assert
        Assert.AreEqual(expected, job.ServicePullRequest);
    }
}