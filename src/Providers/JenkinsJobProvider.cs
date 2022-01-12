#nullable enable
using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Utilities;

namespace coveralls_uploader.Providers
{
   public class JenkinsJobProvider : IEnvironmentVariablesJobProvider
   {
       public string ServiceName => "jenkins";
   
       private readonly IEnvironmentWrapper _environment = null!;
   
       public JenkinsJobProvider()
       {
       }
       
       public JenkinsJobProvider(IEnvironmentWrapper environment)
       {
           _environment = environment;
       }

       public Job Load()
       {
           var job = new Job
           {
               RepositoryToken = _environment.GetEnvironmentVariable("COVERALLS_TOKEN"),
               ServiceName = ServiceName,
               ServiceNumber = _environment.GetEnvironmentVariable("BUILD_NUMBER"),
               CommitSha = _environment.GetEnvironmentVariable("GIT_COMMIT"),
               ServicePullRequest = _environment.GetEnvironmentVariable("COVERALLS_PULL_REQUEST_NUMBER"),
               Git = new Git
               {
                   Head = new Head
                   {
                       Id = _environment.GetEnvironmentVariable("GIT_COMMIT"),
                       AuthorEmail = _environment.GetEnvironmentVariable("GIT_AUTHOR_EMAIL"),
                       AuthorName = _environment.GetEnvironmentVariable("GIT_AUTHOR_NAME"),
                       CommitterEmail = _environment.GetEnvironmentVariable("GIT_COMMITTER_EMAIL"),
                       CommitterName = _environment.GetEnvironmentVariable("GIT_COMMITTER_NAME"),
                   },
                   Branch = GetBranch()
               }
           };

           return job;
       }

       private string? GetBranch()
       {
           var branch = _environment.GetEnvironmentVariable("GIT_BRANCH");

           return branch != null && branch.Contains('/') ? branch.Split('/')[1] : branch;
       }
   } 
}

