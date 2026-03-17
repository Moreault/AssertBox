namespace AssertBox.UnitTests;

[TestClass]
public class StringAssertionTests
{
    [TestMethod]
    public void Contain_WithMatchingSubstring_ShouldPass()
    {
        "hello world".Should().Contain("world");
    }

    [TestMethod]
    public void Contain_WithMissingSubstring_ShouldFail()
    {
        Action act = () => "hello world".Should().Contain("xyz");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotContain_WithMissingSubstring_ShouldPass()
    {
        "hello world".Should().NotContain("xyz");
    }

    [TestMethod]
    public void StartWith_WithMatchingPrefix_ShouldPass()
    {
        "hello world".Should().StartWith("hello");
    }

    [TestMethod]
    public void StartWith_WithNonMatchingPrefix_ShouldFail()
    {
        Action act = () => "hello world".Should().StartWith("world");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void EndWith_WithMatchingSuffix_ShouldPass()
    {
        "hello world".Should().EndWith("world");
    }

    [TestMethod]
    public void EndWith_WithNonMatchingSuffix_ShouldFail()
    {
        Action act = () => "hello world".Should().EndWith("hello");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Match_WithMatchingPattern_ShouldPass()
    {
        "abc123".Should().Match(@"^[a-z]+\d+$");
    }

    [TestMethod]
    public void Match_WithNonMatchingPattern_ShouldFail()
    {
        Action act = () => "abc".Should().Match(@"^\d+$");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeEmpty_WithEmptyString_ShouldPass()
    {
        "".Should().BeEmpty();
    }

    [TestMethod]
    public void BeEmpty_WithNonEmptyString_ShouldFail()
    {
        Action act = () => "hello".Should().BeEmpty();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeEmpty_WithNonEmptyString_ShouldPass()
    {
        "hello".Should().NotBeEmpty();
    }

    [TestMethod]
    public void HaveLength_WithCorrectLength_ShouldPass()
    {
        "hello".Should().HaveLength(5);
    }

    [TestMethod]
    public void HaveLength_WithIncorrectLength_ShouldFail()
    {
        Action act = () => "hello".Should().HaveLength(3);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeEquivalentTo_CaseInsensitive_ShouldPass()
    {
        "Hello".Should().BeEquivalentTo("hello");
    }

    [TestMethod]
    public void BeNullOrEmpty_WithEmpty_ShouldPass()
    {
        "".Should().BeNullOrEmpty();
    }

    [TestMethod]
    public void NotBeNullOrEmpty_WithContent_ShouldPass()
    {
        "hello".Should().NotBeNullOrEmpty();
    }

    [TestMethod]
    public void BeNullOrWhiteSpace_WithWhiteSpace_ShouldPass()
    {
        "   ".Should().BeNullOrWhiteSpace();
    }

    [TestMethod]
    public void BeNullOrWhiteSpace_WithEmpty_ShouldPass()
    {
        "".Should().BeNullOrWhiteSpace();
    }

    [TestMethod]
    public void BeNullOrWhiteSpace_WithContent_ShouldFail()
    {
        Action act = () => "hello".Should().BeNullOrWhiteSpace();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeNullOrWhiteSpace_WithContent_ShouldPass()
    {
        "hello".Should().NotBeNullOrWhiteSpace();
    }

    [TestMethod]
    public void NotBeNullOrWhiteSpace_WithWhiteSpace_ShouldFail()
    {
        Action act = () => "   ".Should().NotBeNullOrWhiteSpace();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeNullOrWhiteSpace_WithEmpty_ShouldFail()
    {
        Action act = () => "".Should().NotBeNullOrWhiteSpace();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Chaining_MultipleStringAssertions()
    {
        "hello world".Should().Contain("hello").EndWith("world").HaveLength(11);
    }
}
