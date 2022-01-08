using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using coveralls_uploader.Models;
using coveralls_uploader.Services;
using Moq;
using NUnit.Framework;
using Serilog;

namespace Tests.Services
{
    public class SourceFileServiceTests
    {
        private const string TestFilePath = "/test.cs";
        private const string TestFileContent = "This is a test file!";

        private SourceFileService _sut;
        
        private readonly MockFileSystem _fileSystemMock = new(new Dictionary<string, MockFileData>
        {
            {TestFilePath, new MockFileData(TestFileContent)}
        });

        private Mock<ILogger> _loggerMock;
        
        [SetUp]
        public void Setup()
        {
            _loggerMock = new();
            _sut = new SourceFileService(_fileSystemMock, _loggerMock.Object);
        }
        
        [Test]
        public async Task WhenICreateAsync_ThenItWorks()
        {
            // Arrange
            var fileCoverage = new FileCoverage(TestFilePath);
            var commandOptions = new CommandOptions();

            // Act
            var sourceFiles = await _sut.CreateManyAsync(
                new List<FileCoverage> {fileCoverage},
                commandOptions);

            // Assert
            Assert.IsNotEmpty(sourceFiles);
            Assert.AreEqual(1, sourceFiles.Count);
            Assert.IsNull(sourceFiles.First().Source);
            Assert.IsNotNull(sourceFiles.First().BranchCoverage);
            Assert.IsNotNull(sourceFiles.First().LineCoverage);
            Assert.IsNotEmpty(sourceFiles.First().Digest);
            Assert.IsNotEmpty(sourceFiles.First().Name);
        }
        
        [Test]
        public async Task WhenICreateAsync_WithSourceOption_ThenItIncludesTheSourceFileContent()
        {
            // Arrange
            var fileCoverage = new FileCoverage(TestFilePath);
            var commandOptions = new CommandOptions
            {
                Source = true
            };

            // Act
            var sourceFiles = await _sut.CreateManyAsync(
                new List<FileCoverage> {fileCoverage},
                commandOptions);

            // Assert
            Assert.IsNotEmpty(sourceFiles);
            Assert.AreEqual(1, sourceFiles.Count);
            Assert.AreEqual(TestFileContent, sourceFiles.First().Source);
        }
        
        [Test]
        public async Task WhenICreateAsync_AndUnableToOpenFile_ThenItLogsAWarning()
        {
            // Arrange
            var fileCoverage = new FileCoverage("I don't exist.cs");
            var commandOptions = new CommandOptions()
            {
                Source = true
            };

            // Act
            var sourceFiles = await _sut.CreateManyAsync(
                new List<FileCoverage> {fileCoverage},
                commandOptions);

            // Assert
            Assert.IsNotEmpty(sourceFiles);
            Assert.AreEqual(1, sourceFiles.Count);
            Assert.IsNull(sourceFiles.First().Source);

            _loggerMock.Verify(
                logger => logger.Warning(It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
        }
    }
}