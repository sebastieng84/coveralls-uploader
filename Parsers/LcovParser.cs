using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace coveralls_uploader.Parsers
{
    public class LcovParser : IParser
    {
        private const string LcovExtension = ".info";
        private const string LineCoveragePattern = "^DA:(\\d+),(\\d+)";
        private const string BranchCoveragePattern = "^BRDA:(\\d+),(\\d+),(\\d+),(\\d+)";
        private const string SourceFilePattern = "^SF:(,*)";
        
        public IList<SourceFileCoverage> Parse(string filePath)
        {
            ValidateFileExtension(filePath);
            
            var sourceFileCoverageByFile = new Dictionary<string, SourceFileCoverage?>();
            SourceFileCoverage? currentSourceFileCoverage = null;
            
            foreach (var line in File.ReadLines(filePath))
            {
                if (Regex.Match(line, SourceFilePattern) is var sourceFileMatch && sourceFileMatch.Success)
                {
                    var fileName = sourceFileMatch.Groups[1].Value;
                    if (!sourceFileCoverageByFile.TryGetValue(fileName, out currentSourceFileCoverage))
                    {
                        currentSourceFileCoverage = new SourceFileCoverage(fileName);
                    }

                    continue;
                }
                
                if (Regex.Match(line, LineCoveragePattern) is var lineMatch && lineMatch.Success)
                {
                    var lineNumber = int.Parse(lineMatch.Groups[1].Value);
                    var hitCount = int.Parse(lineMatch.Groups[2].Value);
                    
                    currentSourceFileCoverage!.CoverageByLine[lineNumber] = hitCount;
                    
                    continue;
                }

                if (Regex.Match(line, BranchCoveragePattern) is var branchMatch && branchMatch.Success)
                {
                    var lineNumber = int.Parse(lineMatch.Groups[1].Value);
                    var blockNumber = int.Parse(lineMatch.Groups[2].Value);
                    var branchNumber = int.Parse(lineMatch.Groups[3].Value);
                    var hitCount = int.Parse(lineMatch.Groups[4].Value);

                    currentSourceFileCoverage!.BranchCoverages.Add(new BranchCoverage(
                        lineNumber,
                        blockNumber,
                        branchNumber,
                        hitCount));
                }
            }

            return sourceFileCoverageByFile.Values.ToList()!;
        }

        private static void ValidateFileExtension(string filePath)
        {
            // TODO: Validate filepath
            if (Path.GetExtension(filePath) != LcovExtension)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(filePath),
                    $"Unsupported file extension. File extension must be {LcovExtension}");
            }
        }
    }
}