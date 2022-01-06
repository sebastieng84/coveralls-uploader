using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Models.Coveralls.Git;
using coveralls_uploader.Utilities;

namespace coveralls_uploader.JobProviders
{
    public class GitHubJobProvider : IEnvironmentVariablesJobProvider
    {
        public string ServiceName => "github";

        private readonly IEnvironmentWrapper _environment;

        public GitHubJobProvider()
        {
        }
    
        public GitHubJobProvider(IEnvironmentWrapper environment)
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
                CommitSha = _environment.GetEnvironmentVariable("GITHUB_SHA"),
                GitInformation = new GitInformation
                {
                    Head = new Head
                    {
                        Id = _environment.GetEnvironmentVariable("GITHUB_SHA"),
                        AuthorEmail = _environment.GetEnvironmentVariable("GIT_COMMIT_AUTHOR_EMAIL"),
                        AuthorName = _environment.GetEnvironmentVariable("GIT_COMMIT_AUTHOR_NAME"),
                        CommitterEmail = _environment.GetEnvironmentVariable("GIT_COMMIT_COMMITTER_EMAIL"),
                        CommitterName = _environment.GetEnvironmentVariable("GIT_COMMIT_COMMITTER_NAME"),
                        Message = _environment.GetEnvironmentVariable("GIT_COMMIT_MESSAGE_SUBJECT_SANITIZED")
                    },
                    // TODO: Add Remotes 
                    Branch = _environment.GetEnvironmentVariable("GITHUB_REF")
                }
            };
        }
    }
}
