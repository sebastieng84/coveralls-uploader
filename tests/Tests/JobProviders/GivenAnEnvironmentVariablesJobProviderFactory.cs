using System;
using coveralls_uploader.JobProviders;
using coveralls_uploader.Utilities;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;

namespace Tests.JobProviders;

public class GivenAnEnvironmentVariablesJobProviderFactory
{
    private EnvironmentVariablesJobProviderFactory _sut;

    private Mock<IHost> _hostMock;
    private Mock<IEnvironmentWrapper> _environmentWrapperMock;

    [SetUp]
    public void Setup()
    {
        _hostMock = new();
        _environmentWrapperMock = new();
        _sut = new EnvironmentVariablesJobProviderFactory(_hostMock.Object, _environmentWrapperMock.Object);
    }

    [Test]
    public void WhenICreate_AndIsAJenkinsEnvironment_ThenItReturnsAJenkinsJobProvider()
    {
        // Given
        _hostMock
            .Setup(host => host.Services.GetService(typeof(JenkinsJobProvider)))
            .Returns(new Mock<JenkinsJobProvider>().Object);
        _environmentWrapperMock
            .Setup(wrapper => wrapper.GetEnvironmentVariable(EnvironmentVariablesJobProviderFactory.JenkinsEnvironmentVariable))
            .Returns("not empty");

        // When
        var provider = _sut.Create();
        
        // Then
        Assert.IsInstanceOf<JenkinsJobProvider>(provider);
    }
    
    [Test]
    public void WhenICreate_AndIsAGitHubEnvironment_ThenItReturnsAJenkinsJobProvider()
    {
        // Given
        _hostMock
            .Setup(host => host.Services.GetService(typeof(GitHubJobProvider)))
            .Returns(new Mock<GitHubJobProvider>().Object);
        _environmentWrapperMock
            .Setup(wrapper => wrapper.GetEnvironmentVariable(EnvironmentVariablesJobProviderFactory.GitHubEnvironmentVariable))
            .Returns("not empty");

        // When
        var provider = _sut.Create();
        
        // Then
        Assert.IsInstanceOf<GitHubJobProvider>(provider);
    }
    
    [Test]
    public void WhenICreate_AndIsUnsupportedEnvironment_ThenItThrows()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _sut.Create());
    }
}