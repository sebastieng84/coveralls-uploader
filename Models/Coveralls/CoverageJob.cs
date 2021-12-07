using System;
using System.Collections.Generic;
using coveralls_uploader.Models.Coveralls.Git;
using Newtonsoft.Json;

namespace coveralls_uploader.Models.Coveralls
{
    public class CoverageJob
    {
        [JsonProperty("repo_token")]
        public string RepositoryToken { get; set; }
        [JsonProperty("service_name")]
        public string ServiceName { get; set; }
        [JsonProperty("service_number")]
        public string ServiceNumber { get; set; }
        [JsonProperty("service_job_id")]
        public string ServiceJobId { get; set; }
        [JsonProperty("service_pull_request")]
        public string ServicePullRequest { get; set; }
        [JsonProperty("source_files")]
        public IList<SourceFileCoverage> SourceFiles {get; set;}
        public bool Parallel { get; set; }
        [JsonProperty("flag_name")]
        public string FlagName { get; set; }
        [JsonProperty("commit_sha")]
        public string CommitSha { get; set; }
        [JsonProperty("run_at")]
        public DateTime RunAt { get; set; }
        [JsonProperty("git")]
        public GitInformation GitInformation { get; set; }
        
        public CoverageJob(
            string repositoryToken,
            string serviceName, 
            string serviceNumber, 
            string serviceJobId, 
            string servicePullRequest,
            bool parallel, 
            string flagName, 
            string commitSha,
            IList<SourceFileCoverage> sourceFiles, 
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
            RunAt = runAt;
            GitInformation = gitInformation;
        }
    }
}