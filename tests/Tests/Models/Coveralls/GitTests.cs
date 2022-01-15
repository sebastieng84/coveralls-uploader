using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using coveralls_uploader.Models.Coveralls;
using NUnit.Framework;

namespace Tests.Models.Coveralls;

public class GitTests
{
    private readonly Fixture _fixture = new();
    
    [Test]
    public void WhenIMerge_ThenItUpdatesNullValues()
    {
        // Arrange
        var git = new Git();
        var other = _fixture.Create<Git>();

        // Act
        git.Merge(other);
        
        // Assert
        Assert.AreEqual(git, other);
    }
    
    [Test]
    public void WhenIMerge_AndItHasValues_ThenItDoesNotUpdate()
    {
        // Arrange
        const string branch = "dev";
        var head = new Head();
        var remotes = new List<Remote> {new("name", "url")};

        var git = new Git
        {
            Branch = branch, 
            Head = head,
            Remotes = remotes
        };
        var other = new Git();

        // Act
        git.Merge(other);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.AreEqual(branch, git.Branch);
            Assert.AreEqual(head, git.Head);
            CollectionAssert.AreEqual(remotes, git.Remotes);
        });
    }
    
    [Test]
    public void WhenIMerge_WithNull_ThenItDoesNotThrow()
    {
        // Arrange
        var git = new Git();

        // Act
        // Assert
        Assert.DoesNotThrow(() => git.Merge(null));
    }

    [Test]
    public void WhenIGetHashCode_WithTwoDifferentObjects_ThenTheyAreNotEqual()
    {
        // Arrange
        var git = _fixture.Create<Git>();
        var other = _fixture.Create<Git>();

        // Act
        var hashCode = git.GetHashCode();
        var otherHashCode = other.GetHashCode();

        // Assert
        Assert.AreNotEqual(hashCode, otherHashCode);
    }
    
    [Test]
    public void WhenIEquals_WithTwoObjetsWithIdenticalValues_ThenTrue()
    {
        // Arrange
        var gits = _fixture.Build<Git>()
            .With(x => x.Branch, "dev")
            .With(x => x.Remotes, new List<Remote> {new ("name", "url")})
            .With(x => x.Head, new Head())
            .CreateMany(2)
            .ToList();

        // Act
        var equal = gits[0].Equals(gits[1]);

        // Assert
        Assert.IsTrue(equal);
    }
    
    [Test]
    public void WhenIEquals_WithTwoDifferentObjets_ThenFalse()
    {
        // Arrange
        var git = _fixture.Create<Git>();
        var other = _fixture.Create<Git>();

        // Act
        var equal = git.Equals(other);

        // Assert
        Assert.IsFalse(equal);
    }
    
    [Test]
    public void WhenIEquals_WithADifferentType_ThenFalse()
    {
        // Arrange
        var git = _fixture.Create<Git>();
        var job = new Job();

        // Act
        var equal = git.Equals(job);

        // Assert
        Assert.IsFalse(equal);
    }
}