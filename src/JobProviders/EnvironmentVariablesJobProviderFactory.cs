using coveralls_uploader.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace coveralls_uploader.JobProviders;

public class EnvironmentVariablesJobProviderFactory
{
    public const string JenkinsEnvironmentVariable = "JENKINS_HOME";
    public const string GitHubEnvironmentVariable = "GITHUB_ACTIONS";
    
    private readonly IHost _host;
    private readonly IEnvironmentWrapper _environment;

    public EnvironmentVariablesJobProviderFactory(
        IHost host,
        IEnvironmentWrapper environment)
    {
        _host = host;
        _environment = environment;
    }

    public IEnvironmentVariablesJobProvider Create()
    {
        if (IsJenkinsEnvironment())
        {
            return _host.Services.GetRequiredService<JenkinsJobProvider>();
        }

        if (IsGitHubEnvironment())
        {
            return _host.Services.GetRequiredService<GitHubJobProvider>();
        }

        throw new ArgumentOutOfRangeException();
    }

    private bool IsJenkinsEnvironment()
    {
        return !string.IsNullOrEmpty(_environment.GetEnvironmentVariable(JenkinsEnvironmentVariable));
    }
    
    private bool IsGitHubEnvironment()
    {
        return !string.IsNullOrEmpty(_environment.GetEnvironmentVariable(GitHubEnvironmentVariable));
    }
}