using System.Linq;
using coveralls_uploader.Commands;
using NUnit.Framework;

namespace Tests.Commands;

public class UploadCommandTests
{
    [Test]
    public void WhenICreate_ItShouldHaveAllTheOptions()
    {
        // Arrange
        var optionNames = new[] {"verbose", "input", "token", "source"};
        
        // Act
        var uploadCommand = new UploadCommand();
        
        // Assert
        Assert.IsTrue(optionNames.All(uploadCommand.Options.Select(option => option.Name).Contains));
    }
}