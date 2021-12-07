using System.Collections.Generic;

namespace coveralls_uploader.Parsers
{
    public class SourceFileCoverage
    {
        public string Name { get; }
        public LineCoverageDictionary CoverageByLine { get; } = new();
        public BranchCoverageList BranchCoverages { get;} = new();
        public string Source { get; set; }
        
        public SourceFileCoverage(string name)
        {
            Name = name;
        }
    }
}