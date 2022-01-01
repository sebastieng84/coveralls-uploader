using System.IO.Abstractions;
using coveralls_uploader.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Tests.Services
{
    public class GivenASourceFileSystem
    {
        private SourceFileService _sut;
        private readonly Mock<IFileSystem> _fileSystemMock = new();
        private readonly Mock<ILogger> _loggerMock = new();
        
        [SetUp]
        public void Setup()
        {
            _sut = new SourceFileService(_fileSystemMock.Object, _loggerMock.Object);
        }
        
        [Test]
        public void WhenICreateAsync_ThenItWorks()
        {
            // Given
            
            // When
            
            // Then
        }
    }
}