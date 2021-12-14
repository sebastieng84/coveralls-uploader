using System.Collections.Generic;
using System.Linq;

namespace coveralls_uploader.Models
{
    public class LineCoverageDictionary : Dictionary<int, int>
    {
        public int?[] ToArray()
        {
            var maxValue = Keys.Max(key => key);
            
            var lineCoverageArray = new int?[maxValue];
            foreach (var key in Keys)
            {
                lineCoverageArray[key - 1] = this[key];
            }

            return lineCoverageArray;
        }
    }
}