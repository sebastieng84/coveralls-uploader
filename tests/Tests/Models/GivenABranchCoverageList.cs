using coveralls_uploader.Models;
using NUnit.Framework;

namespace Tests.Models
{
    public class GivenABranchCoverageList
    {
        [Test]
        public void WhenIToArray_WithAnEmptyList_ThenItReturnsAnEmptyArray()
        {
            // Arrange
            var branchCoverageList = new BranchCoverageList();

            // Act
            var array = branchCoverageList.ToArray();

            // Assert
            Assert.IsEmpty(array);
        }
        
        [Test]
        public void WhenIToArray_ThenItReturnsAnArrayWithTheFieldsInTheProperOrder()
        {
            // Arrange
            const int lineNumber = 1;
            const int blockNumber = 2;
            const int branchNumber = 3;
            const int hitCount = 4;

            var branchCoverageList = new BranchCoverageList
            {
                new(lineNumber,
                    blockNumber,
                    branchNumber,
                    hitCount)
            };

            // Act
            var array = branchCoverageList.ToArray();

            // Assert
            Assert.AreEqual(lineNumber, array[0]);
            Assert.AreEqual(blockNumber, array[1]);
            Assert.AreEqual(branchNumber, array[2]);
            Assert.AreEqual(hitCount, array[3]);
        }
        
        [Test]
        public void WhenIToArray_ThenItReturnsAnArrayWithTheProperSize()
        {
            // Arrange
            var branchCoverageList = new BranchCoverageList
            {
                new(),
                new()
            };

            // Act
            var array = branchCoverageList.ToArray();

            // Assert
            Assert.AreEqual(branchCoverageList.Count * 4, array.Length);
        }
    }
}