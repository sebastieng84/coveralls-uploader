using System;
using System.Text.RegularExpressions;
using coveralls_uploader.Models.Coveralls;
using coveralls_uploader.Models.Coveralls.Git;
using coveralls_uploader.Utilities;
using Microsoft.Extensions.Logging;

namespace coveralls_uploader.JobProviders
{
   public class JenkinsJobProvider : IEnvironmentVariablesJobProvider
   {
       public string ServiceName => "jenkins";
   
       private readonly IEnvironmentWrapper _environment;
   
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
               RepositoryToken =  _environment.GetEnvironmentVariable("COVERALLS_TOKEN"),
               ServiceName = ServiceName,
               ServiceNumber = _environment.GetEnvironmentVariable("BUILD_NUMBER"),
               CommitSha = _environment.GetEnvironmentVariable("GIT_COMMIT"),
               GitInformation = new GitInformation
               {
                   Head = new Head
                   {
                       Id = _environment.GetEnvironmentVariable("GIT_COMMIT"),
                       AuthorEmail = _environment.GetEnvironmentVariable("GIT_AUTHOR_EMAIL"),
                       AuthorName = _environment.GetEnvironmentVariable("GIT_AUTHOR_NAME"),
                       CommitterEmail = _environment.GetEnvironmentVariable("GIT_COMMITTER_EMAIL"),
                       CommitterName = _environment.GetEnvironmentVariable("GIT_COMMITTER_NAME"),
                       // TODO: Find a way to retrieve commit message
                   },
                   Branch = _environment.GetEnvironmentVariable("GIT_BRANCH")
               }
           };
   
           try
           {
               var repositoryUrl = _environment.GetEnvironmentVariable("GIT_URL");
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
}

