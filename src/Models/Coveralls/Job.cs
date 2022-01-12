using System.Collections.Generic;
using Newtonsoft.Json;

namespace coveralls_uploader.Models.Coveralls
{
    public class Job
    {
        [JsonProperty("repo_token")] 
        public string RepositoryToken { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNumber { get; set; }
        public string ServiceJobId { get; set; }
        public string ServicePullRequest { get; set; }
        public IList<SourceFile> SourceFiles { get; set; }
        [JsonIgnore] 
        public bool Parallel { get; set; }
        public string FlagName { get; set; }
        public string CommitSha { get; set; }
        public string RunAt { get; set; }
        public Git Git { get; set; }

        public Job()
        {
        }
    }
}