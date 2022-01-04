using System.Text.RegularExpressions;
using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Models.Coveralls.Git;

namespace coveralls_uploader.JobProviders;

public class JenkinsJobProvider : IEnvironmentVariablesJobProvider
{
    private const string JenkinsServiceName = "jenkins";
    public Job Load()
    {
        var job = new Job
        {
            RepositoryToken =  Environment.GetEnvironmentVariable(""),
            ServiceName = JenkinsServiceName,
            ServiceNumber = Environment.GetEnvironmentVariable("BUILD_NUMBER"),
            CommitSha = Environment.GetEnvironmentVariable("GIT_COMMIT"),
            GitInformation = new GitInformation
            {
                Head = new Head
                {
                    Id = Environment.GetEnvironmentVariable("GIT_COMMIT"),
                    AuthorEmail = Environment.GetEnvironmentVariable("GIT_AUTHOR_EMAIL"),
                    AuthorName = Environment.GetEnvironmentVariable("GIT_AUTHOR_NAME"),
                    CommitterEmail = Environment.GetEnvironmentVariable("GIT_COMMITTER_EMAIL"),
                    CommitterName = Environment.GetEnvironmentVariable("GIT_COMMITTER_NAME"),
                    //Message = Environment.GetEnvironmentVariable("")
                },
                Branch = Environment.GetEnvironmentVariable("GIT_BRANCH")
            }
        };

        try
        {
            var repositoryUrl = Environment.GetEnvironmentVariable("GIT_URL");
            var repositoryName = Regex.Match(repositoryUrl, @"^.*\/([^\/]+?).git$").Groups[1].Value;

            job.GitInformation.Remotes.Add(new Remote(repositoryName, repositoryUrl));
        }
        catch (Exception)
        {
            // ignored
        }

        return job;
    }
}