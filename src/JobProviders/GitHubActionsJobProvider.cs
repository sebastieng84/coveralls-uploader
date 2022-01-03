using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Models.Coveralls.Git;

namespace coveralls_uploader.JobProviders;

public class GitHubActionsJobProvider : IEnvironmentVariablesJobProvider
{
    private const string GithubActionsServiceName = "github";

    public Job Load()
    {
        return new Job
        {
            RepositoryToken =  Environment.GetEnvironmentVariable("GITHUB_TOKEN"),
            ServiceName = GithubActionsServiceName,
            ServiceNumber = Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER"),
            ServiceJobId = Environment.GetEnvironmentVariable("GITHUB_RUN_ID"),
            CommitSha = Environment.GetEnvironmentVariable("GITHUB_SHA"),
            GitInformation = new GitInformation
            {
                Head = new Head
                {
                    Id = Environment.GetEnvironmentVariable("GITHUB_SHA"),
                    AuthorEmail = Environment.GetEnvironmentVariable("GIT_COMMIT_AUTHOR_EMAIL"),
                    AuthorName = Environment.GetEnvironmentVariable("GIT_COMMIT_AUTHOR_NAME"),
                    CommitterEmail = Environment.GetEnvironmentVariable("GIT_COMMIT_COMMITTER_EMAIL"),
                    CommitterName = Environment.GetEnvironmentVariable("GIT_COMMIT_COMMITTER_NAME"),
                    Message = Environment.GetEnvironmentVariable("GIT_COMMIT_MESSAGE_BODY")
                },
                // TODO: Add Remotes 
                Branch = Environment.GetEnvironmentVariable("GITHUB_REF")
            }
        };
    }
}