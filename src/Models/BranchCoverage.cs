namespace coveralls_uploader.Models
{
    public class BranchCoverage
    {
        public int LineNumber { get; set; }
        public int BlockNumber { get; set; }
        public int BranchNumber { get; set; }
        public int HitCount { get; set; }

        public BranchCoverage()
        {
        }

        public BranchCoverage(
            int lineNumber,
            int blockNumber,
            int branchNumber,
            int hitCount)
        {
            LineNumber = lineNumber;
            BlockNumber = blockNumber;
            BranchNumber = branchNumber;
            HitCount = hitCount;
        }
    }
}