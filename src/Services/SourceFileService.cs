using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coveralls_uploader.Models;
using coveralls_uploader.Models.Coveralls;
using Serilog;

namespace coveralls_uploader.Services
{
    public class SourceFileService
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _logger;
            
        public SourceFileService(
            IFileSystem fileSystem, 
            ILogger logger)
        {
            _fileSystem = fileSystem;
            _logger = logger;
        }
    
        public async Task<IList<SourceFile>> CreateManyAsync(
            IList<FileCoverage> fileCoverages, 
            CommandOptions commandOptions)
        {
            return await Task.WhenAll(fileCoverages.Select(async file => await CreateAsync(file, commandOptions)));
        }
        
        private async Task<SourceFile> CreateAsync(FileCoverage fileCoverage, CommandOptions commandOptions)
        {
            string fileContent = null;
            string md5Hash = null;
                
            var filePath = GetRelativeFilePath(fileCoverage.FilePath);
            try
            {
                fileContent = commandOptions.Source
                    ? await _fileSystem.File.ReadAllTextAsync(fileCoverage.FilePath, Encoding.UTF8)
                    : null;
                md5Hash = GetMd5Digest(fileCoverage.FilePath);
            }
            catch (Exception)
            {
                _logger.Warning("Unable to read file's content: {FilePath}", filePath);
            }
    
            return new SourceFile(
                filePath,
                md5Hash,
                fileCoverage.CoverageByLine.ToArray(),
                fileCoverage.BranchCoverages.ToArray(),
                fileContent);
        }
    
        private string GetMd5Digest(string filePath)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            var hashBytes = md5.ComputeHash(_fileSystem.File.OpenRead(filePath));
                
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
        }
    
        private string GetRelativeFilePath(string filePath)
        {
            var currentDirectory = _fileSystem.Directory.GetCurrentDirectory();
    
            return _fileSystem.Path.GetRelativePath(currentDirectory, filePath);
        }
    }
}