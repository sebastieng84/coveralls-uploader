using coveralls_uploader.Models;
using NUnit.Framework;

namespace Tests.Models
{
    public class GivenALineCoverageDictionary
    {
        [Test]
        public void WhenIToArray_ItReturnsAnArrayUpToTheMaxKey()
        {
            // Given
            const int maxKey = 100;
            
            var lineCoverageDictionary = new LineCoverageDictionary
            {
                {1, 1},
                {27, 3},
                {maxKey, 0}
            };
            
            // When
            var array = lineCoverageDictionary.ToArray();
            
            // Then
            Assert.AreEqual(maxKey, array.Length);
        }
        
        [Test]
        public void WhenIToArray_WithAnEmptyDictionary_ItReturnsAnEmptyArray()
        {
            // Given
            var lineCoverageDictionary = new LineCoverageDictionary();

            // When
            var array = lineCoverageDictionary.ToArray();
            
            // Then
            Assert.IsEmpty(array);
        }
    }
}