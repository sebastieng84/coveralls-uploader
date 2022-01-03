using coveralls_uploader.Models;
using NUnit.Framework;

namespace Tests.Models
{
    public class GivenALineCoverageDictionary
    {
        [Test]
        public void WhenIToArray_ItReturnsAnArrayUpToTheMaxKey()
        {
            // Arrange
            const int maxKey = 100;
            
            var lineCoverageDictionary = new LineCoverageDictionary
            {
                {1, 1},
                {27, 3},
                {maxKey, 0}
            };
            
            // Act
            var array = lineCoverageDictionary.ToArray();
            
            // Assert
            Assert.AreEqual(maxKey, array.Length);
        }
        
        [Test]
        public void WhenIToArray_WithAnEmptyDictionary_ItReturnsAnEmptyArray()
        {
            // Arrange
            var lineCoverageDictionary = new LineCoverageDictionary();

            // Act
            var array = lineCoverageDictionary.ToArray();
            
            // Assert
            Assert.IsEmpty(array);
        }
    }
}