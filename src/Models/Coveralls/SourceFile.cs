using Newtonsoft.Json;

namespace coveralls_uploader.Models.Coveralls
{
    public class SourceFile
    {
        public string Name { get; set; }
        [JsonProperty("source_digest")]
        public string Digest { get; set; }
        [JsonProperty("coverage")]
        public int?[] LineCoverage { get; set; }
        [JsonIgnore]
        public int[] BranchCoverage { get; set; }
        public string Source { get; set; }
        
        public SourceFile(
            string name, 
            string digest, 
            int?[] lineCoverage, 
            int[] branchCoverage, 
            string source)
        {
            Name = name;
            Digest = digest;
            LineCoverage = lineCoverage;
            BranchCoverage = branchCoverage;
            Source = source;
        }
    }
}