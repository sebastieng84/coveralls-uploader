using System;
using System.IO;
using System.IO.Abstractions;
using System.Text;
using System.Threading.Tasks;
using coveralls_uploader.Models;
using coveralls_uploader.Models.Coveralls;

namespace coveralls_uploader.Services
{
    public class SourceFileService
    {
        private readonly IFileSystem _fileSystem;
        
        public SourceFileService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public async Task<SourceFile> CreateAsync(FileCoverage fileCoverage)
        {
            string fileContent = null;
            string md5Hash = null;
            
            var filePath = GetRelativeFilePath(fileCoverage.FilePath);
            try
            {
                fileContent = await _fileSystem.File.ReadAllTextAsync(fileCoverage.FilePath, Encoding.UTF8);
                md5Hash = GetMd5Digest(fileCoverage.FilePath);
            }
            catch (Exception)
            {
                Console.WriteLine("lol");
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