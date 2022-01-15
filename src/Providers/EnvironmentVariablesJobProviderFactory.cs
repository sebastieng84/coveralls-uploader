using System;
using coveralls_uploader.Utilities;
using coveralls_uploader.Utilities.Wrappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace coveralls_uploader.Providers
{
    public class EnvironmentVariablesJobProviderFactory
    {
        private const string JenkinsEnvironmentVariable = "JENKINS_HOME";
        private const string GitHubEnvironmentVariable = "GITHUB_ACTIONS";

        private readonly IHost _host;
        private readonly IEnvironment _environment;

        public EnvironmentVariablesJobProviderFactory(
            IHost host,
            IEnvironment environment)
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
}