namespace coveralls_uploader.Models
{
    public class FileCoverage
    {
        public string FilePath { get; }
        public LineCoverageDictionary CoverageByLine { get; } = new();
        public BranchCoverageList BranchCoverages { get; } = new();
        public string Source { get; set; }

        public FileCoverage(string filePath)
        {
            FilePath = filePath;
        }
    }
}