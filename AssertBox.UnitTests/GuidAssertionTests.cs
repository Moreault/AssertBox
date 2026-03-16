namespace AssertBox.UnitTests;

[TestClass]
public class GuidAssertionTests
{
    [TestMethod]
    public void BeEmpty_WithEmptyGuid_ShouldPass()
    {
        Guid.Empty.Should().BeEmpty();
    }

    [TestMethod]
    public void BeEmpty_WithNonEmptyGuid_ShouldFail()
    {
        Action act = () => Guid.NewGuid().Should().BeEmpty();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeEmpty_WithNonEmptyGuid_ShouldPass()
    {
        Guid.NewGuid().Should().NotBeEmpty();
    }

    [TestMethod]
    public void NotBeEmpty_WithEmptyGuid_ShouldFail()
    {
        Action act = () => Guid.Empty.Should().NotBeEmpty();
        act.Should().Throw<AssertBoxException>();
    }
}
