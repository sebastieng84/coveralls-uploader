using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using coveralls_uploader.Models;
using coveralls_uploader.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using static Moq.It;

namespace Tests.Services
{
    public class GivenASourceFileService
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

            _loggerMock.Verify(logger => logger.Log(
                    LogLevel.Warning, 
                    IsAny<EventId>(), 
                    IsAny<IsAnyType>(), 
                    IsAny<Exception?>(), 
                    IsAny<Func<IsAnyType,Exception?,string>>()),
                Times.Once);
        }
    }
}