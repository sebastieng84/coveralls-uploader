using coveralls_uploader.Utilities;
using NUnit.Framework;

namespace Tests.Utilities;

public class GivenAnEnvironmentWrapper
{
    private readonly EnvironmentWrapper _sut = new();

    [Test]
    public void WhenIGetEnvironmentVariable_AndVariableDoesNotExist_ThenItReturnsNull()
    {
        // Given
        const string variableName = "Test";

        // When
        var value = _sut.GetEnvironmentVariable(variableName);
        
        // Then 
        Assert.IsNull(value);
    }
}