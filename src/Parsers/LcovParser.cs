using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using coveralls_uploader.Models;

namespace coveralls_uploader.Parsers
{
    public class LcovParser : IParser
    {
        private const string LcovExtension = ".info";
        private const string LineCoveragePattern = "^DA:(\\d+),(\\d+)";
        private const string BranchCoveragePattern = "^BRDA:(\\d+),(\\d+),(\\d+),(\\d+)";
        private const string SourceFilePattern = "^SF:(.*)";
        
        private readonly Dictionary<string, FileCoverage> _sourceFileCoverageByFile = new();
        
        public IList<FileCoverage> Parse(string filePath)
        {
            ValidateFileExtension(filePath);
            
            FileCoverage currentFileCoverage = null;
            
            foreach (var line in File.ReadLines(filePath))
            {
                if (Regex.Match(line, SourceFilePattern) is var sourceFileMatch && sourceFileMatch.Success)
                {
                    currentFileCoverage = GetOrAddFileCoverage(sourceFileMatch);
                    
                    continue;
                }
                
                if (Regex.Match(line, LineCoveragePattern) is var lineMatch && lineMatch.Success)
                {
                    AddLineCoverage(lineMatch, currentFileCoverage!);
                    
                    continue;
                }

                if (Regex.Match(line, BranchCoveragePattern) is var branchMatch && branchMatch.Success)
                {
                    AddBranchCoverage(branchMatch, currentFileCoverage!);
                }
            }

            return _sourceFileCoverageByFile.Values.ToList();
        }

        private FileCoverage GetOrAddFileCoverage(Match match)
        {
            var fileName = match.Groups[1].Value;
            
            if (_sourceFileCoverageByFile.TryGetValue(fileName, out var fileCoverage))
            {
                return fileCoverage;
            }
            
            fileCoverage = new FileCoverage(fileName);
            _sourceFileCoverageByFile.Add(fileName, fileCoverage);

            return fileCoverage;
        }

        private static void AddLineCoverage(Match match, FileCoverage fileCoverage)
        {
            var lineNumber = int.Parse(match.Groups[1].Value);
            var hitCount = int.Parse(match.Groups[2].Value);
                    
            fileCoverage.CoverageByLine[lineNumber] = hitCount;
        }

        private static void AddBranchCoverage(Match match, FileCoverage fileCoverage)
        {
            var lineNumber = int.Parse(match.Groups[1].Value);
            var blockNumber = int.Parse(match.Groups[2].Value);
            var branchNumber = int.Parse(match.Groups[3].Value);
            var hitCount = int.Parse(match.Groups[4].Value);

            fileCoverage.BranchCoverages.Add(new BranchCoverage(
                lineNumber,
                blockNumber,
                branchNumber,
                hitCount));
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