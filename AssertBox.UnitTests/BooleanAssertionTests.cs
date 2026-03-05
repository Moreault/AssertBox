namespace AssertBox.UnitTests;

[TestClass]
public class BooleanAssertionTests
{
    [TestMethod]
    public void BeTrue_WithTrue_ShouldPass()
    {
        true.Should().BeTrue();
    }

    [TestMethod]
    public void BeTrue_WithFalse_ShouldFail()
    {
        Action act = () => false.Should().BeTrue();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeFalse_WithFalse_ShouldPass()
    {
        false.Should().BeFalse();
    }

    [TestMethod]
    public void BeFalse_WithTrue_ShouldFail()
    {
        Action act = () => true.Should().BeFalse();
        act.Should().Throw<AssertBoxException>();
    }
}
