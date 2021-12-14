using System.IO.Abstractions;
using coveralls_uploader.Services;
using Moq;
using NUnit.Framework;

namespace Tests.Services
{
    public class GivenASourceFileSystem
    {
        private SourceFileService _sut;
        private Mock<IFileSystem> _fileSystemMock;
        
        [SetUp]
        public void Setup()
        {
            _fileSystemMock = new Mock<IFileSystem>();
            _sut = new SourceFileService(_fileSystemMock.Object);
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