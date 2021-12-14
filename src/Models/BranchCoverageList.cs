using System.Collections.Generic;

namespace coveralls_uploader.Models
{
    public class BranchCoverageList : List<BranchCoverage>
    {
        public new int[] ToArray()
        {
            var array = new int[Count * 4];
            var index = 0;
            foreach(var branchCoverage in this)
            {
                array[index++] = branchCoverage.LineNumber;
                array[index++] = branchCoverage.BlockNumber;
                array[index++] = branchCoverage.BranchNumber;
                array[index++] = branchCoverage.HitCount;
            }

            return array;
        }
    }
}