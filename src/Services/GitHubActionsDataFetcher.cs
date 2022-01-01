using coveralls_uploader.Models;
using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Models.Coveralls.Git;

namespace coveralls_uploader.Services;

public class GitHubActionsDataFetcher : IJobDataFetcher
{
    private const string GITHUB_ACTIONS_SERVICE_NAME = "github-actions";
    
    public GitHubActionsDataFetcher()
    {
    }
    
    public Job Fetch(CommandOptions commandOptions)
    {
        return new Job
        {
            RepositoryToken =  commandOptions.Token,
            ServiceName = GITHUB_ACTIONS_SERVICE_NAME,
            ServiceNumber = Environment.GetEnvironmentVariable("GITHUB_ACTION"),
            ServiceJobId = Environment.GetEnvironmentVariable("GITHUB_JOB"),
            CommitSha = Environment.GetEnvironmentVariable("GITHUB_SHA"),
            RunAt = "2021-12-30 10:45:00 -0800",
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