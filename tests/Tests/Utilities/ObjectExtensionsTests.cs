using coveralls_uploader.Utilities;
using NUnit.Framework;

namespace Tests.Utilities;

public class ObjectExtensionsTests
{
    [Test]
    public void WhenIInList_ThenItReturnsAListContainingTheObject()
    {
        // Arrange
        var obj = new object();
        
        // Act
        var actual = obj.InList();

        // Assert
        Assert.AreEqual(1, actual.Count);
        CollectionAssert.Contains(actual, obj);
    }
}