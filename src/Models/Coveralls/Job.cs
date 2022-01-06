using System;
using System.Collections.Generic;
using coveralls_uploader.Models.Coveralls.Git;
using Newtonsoft.Json;

namespace coveralls_uploader.Models.Coveralls
{
    public class Job
    {
        [JsonProperty("repo_token")] public string RepositoryToken { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNumber { get; set; }
        public string ServiceJobId { get; set; }
        public string ServicePullRequest { get; set; }
        public IList<SourceFile> SourceFiles { get; set; }
        [JsonIgnore] public bool Parallel { get; set; }
        public string FlagName { get; set; }
        public string CommitSha { get; set; }
        public string RunAt { get; set; }
        [JsonProperty("git")] public GitInformation GitInformation { get; set; }

        public Job()
        {
        }

        public Job(
            string repositoryToken,
            string serviceName,
            string serviceNumber,
            string serviceJobId,
            string servicePullRequest,
            bool parallel,
            string flagName,
            string commitSha,
            IList<SourceFile> sourceFiles,
            DateTime runAt,
            GitInformation gitInformation)
        {
            RepositoryToken = repositoryToken;
            ServiceName = serviceName;
            ServiceNumber = serviceNumber;
            ServiceJobId = serviceJobId;
            ServicePullRequest = servicePullRequest;
            SourceFiles = sourceFiles;
            Parallel = parallel;
            FlagName = flagName;
            CommitSha = commitSha;
            // RunAt = runAt;
            GitInformation = gitInformation;
        }
    }
}