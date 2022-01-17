using coveralls_uploader.Utilities.Wrappers;
using NUnit.Framework;

namespace Tests.Utilities;

public class EnvironmentWrapperTests
{
    private readonly EnvironmentWrapper _sut = new();

    [Test]
    public void WhenIGetEnvironmentVariable_AndVariableDoesNotExist_ThenItReturnsNull()
    {
        // Arrange
        const string variableName = "Test";

        // Act
        var value = _sut.GetEnvironmentVariable(variableName);
        
        // Assert 
        Assert.IsNull(value);
    }
}