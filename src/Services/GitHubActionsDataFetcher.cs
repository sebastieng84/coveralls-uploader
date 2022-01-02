using coveralls_uploader.Models;
using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Models.Coveralls.Git;

namespace coveralls_uploader.Services;

public class GitHubActionsDataFetcher : IJobDataFetcher
{
    private const string GithubActionsServiceName = "github";
    
    public GitHubActionsDataFetcher()
    {
    }
    
    public Job Fetch(CommandOptions commandOptions)
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