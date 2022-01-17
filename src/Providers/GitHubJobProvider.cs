using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Utilities;
using coveralls_uploader.Utilities.Wrappers;

namespace coveralls_uploader.Providers
{
    public class GitHubJobProvider : IEnvironmentVariablesJobProvider
    {
        public string ServiceName => "github";

        private readonly IEnvironment _environment;

        protected GitHubJobProvider()
        {
        }

        public GitHubJobProvider(IEnvironment environment)
        {
            _environment = environment;
        }

        public Job Load()
        {
            return new Job
            {
                RepositoryToken = _environment.GetEnvironmentVariable("GITHUB_TOKEN"),
                ServiceName = ServiceName,
                ServiceNumber = _environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER"),
                ServiceJobId = _environment.GetEnvironmentVariable("GITHUB_RUN_ID"),
                ServicePullRequest = _environment.GetEnvironmentVariable("COVERALLS_PULL_REQUEST_NUMBER"),
                CommitSha = _environment.GetEnvironmentVariable("GITHUB_SHA"),
                Git = new Git
                {
                    Branch = _environment.GetEnvironmentVariable("GITHUB_REF")
                }
            };
        }
    }
}