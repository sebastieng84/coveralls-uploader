using System.Linq;
using AutoFixture;
using coveralls_uploader.Models.Coveralls;
using NUnit.Framework;

namespace Tests.Models.Coveralls;

public class HeadTests
{
    private readonly Fixture _fixture = new();
    
    [Test]
    public void WhenIMerge_AndHeadIsNull_ThenItDoesNotThrow()
    {
        // Arrange
        var head = new Head();

        // Act
        // Assert
        Assert.DoesNotThrow(() => head.Merge(null));
    }

    [Test]
    public void WhenIMerge_AndHasValues_ThenItDoesNotUpdate()
    {
        // Arrange
        var id = _fixture.Create<string>();
        var authorEmail = _fixture.Create<string>();
        var authorName = _fixture.Create<string>();
        var committerEmail = _fixture.Create<string>();
        var committerName = _fixture.Create<string>();
        var message = _fixture.Create<string>();
        
        var head = new Head
        {
            Id = id,
            AuthorEmail = authorEmail,
            AuthorName = authorName,
            CommitterEmail = committerEmail,
            CommitterName = committerName,
            Message = message
        };
        var other = _fixture.Create<Head>();
        
        // Act
        head.Merge(other);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.AreEqual(id, head.Id);
            Assert.AreEqual(committerEmail, head.CommitterEmail);
            Assert.AreEqual(committerName, head.CommitterName);
            Assert.AreEqual(authorEmail, head.AuthorEmail);
            Assert.AreEqual(authorName, head.AuthorName);
            Assert.AreEqual(message, head.Message);
        });
    }
    
    [Test]
    public void WhenIMerge_ThenItUpdatesNullValues()
    {
        // Arrange
        var head = new Head();
        var other = _fixture.Create<Head>();

        // Act
        head.Merge(other);
        
        // Assert
        Assert.AreEqual(head, other);
    }
    
    [Test]
    public void WhenIEquals_WithTwoDifferentObjects_ThenFalse()
    {
        // Arrange
        var head = _fixture.Create<Head>();
        var other = _fixture.Create<Head>();

        // Act
        var equal = head.Equals(other);
        
        // Assert
        Assert.IsFalse(equal);
    }
    
    [Test]
    public void WhenIEquals_WithTwoIdenticalObjects_ThenTrue()
    {
        // Arrange
        var heads = _fixture.Build<Head>()
            .With(x => x.Id, "Id")
            .With(x => x.AuthorEmail, "AuthorEmail")
            .With(x => x.AuthorName, "AuthorName")
            .With(x => x.CommitterName, "CommitterName")
            .With(x => x.CommitterEmail, "CommitterEmail")
            .With(x => x.Message, "Message")
            .CreateMany(2)
            .ToList();
        var other = _fixture.Create<Head>();

        // Act
        var equal = heads[0].Equals(heads[1]);
        
        // Assert
        Assert.IsTrue(equal);
    }
    
    [Test]
    public void WhenIEquals_WithADifferentType_ThenFalse()
    {
        // Arrange
        var head = _fixture.Create<Head>();
        var job = new Job();

        // Act
        var equal = head.Equals(job);
        
        // Assert
        Assert.IsFalse(equal);
    }
    
    [Test]
    public void WhenIGetHashCode_WithTwoDifferentObjects_ThenTheyAreNotEqual()
    {
        // Arrange
        var heads = _fixture.CreateMany<Head>(2).ToList();

        // Act
        var hashCode = heads[0].GetHashCode();
        var otherHashCode = heads[1].GetHashCode();

        // Assert
        Assert.AreNotEqual(hashCode, otherHashCode);
    }
}