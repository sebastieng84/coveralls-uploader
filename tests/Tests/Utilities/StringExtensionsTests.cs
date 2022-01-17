using AutoFixture;
using coveralls_uploader.Utilities;
using NUnit.Framework;

namespace Tests.Utilities;

public class StringExtensionsTests
{
    private readonly Fixture _fixture = new();
    
    [Test]
    public void WhenIIsNullOrEmpty_WithEmptyString_ItReturnsTrue()
    {
        // Arrange
        var test = string.Empty;
        
        // Act
        var actual = test.IsNullOrEmpty();

        // Assert
        Assert.IsTrue(actual);
    }
    
    [Test]
    public void WhenIIsNullOrEmpty_WithANullString_ItReturnsTrue()
    {
        // Arrange
        string test = null;
        
        // Act
        var actual = test.IsNullOrEmpty();

        // Assert
        Assert.IsTrue(actual);
    }
    
    [Test]
    public void WhenIIsNullOrEmpty_OnAString_ItReturnsFalse()
    {
        // Arrange
        var test = _fixture.Create<string>();
        
        // Act
        var actual = test.IsNullOrEmpty();

        // Assert
        Assert.IsFalse(actual);
    }
}