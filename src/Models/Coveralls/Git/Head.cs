using Newtonsoft.Json;

namespace coveralls_uploader.Models.Coveralls.Git
{
    public class Head
    {
        public string Id { get; set; }
        [JsonProperty("author_name")]
        public string AuthorName { get; set; }
        [JsonProperty("author_email")]
        public string AuthorEmail { get; set; }
        [JsonProperty("committer_name")]
        public string CommitterName { get; set; }
        [JsonProperty("committer_email")]
        public string CommitterEmail { get; set; }
        public string Message { get; set; }

        public Head()
        {
        }
    }
}