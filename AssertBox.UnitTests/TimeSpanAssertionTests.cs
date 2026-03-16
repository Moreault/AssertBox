namespace AssertBox.UnitTests;

[TestClass]
public class TimeSpanAssertionTests
{
    [TestMethod]
    public void BeCloseTo_WithinPrecision_ShouldPass()
    {
        TimeSpan.FromSeconds(5).Should().BeCloseTo(TimeSpan.FromSeconds(5.1), TimeSpan.FromMilliseconds(200));
    }

    [TestMethod]
    public void BeCloseTo_OutsidePrecision_ShouldFail()
    {
        Action act = () => TimeSpan.FromSeconds(5).Should().BeCloseTo(TimeSpan.FromSeconds(6), TimeSpan.FromMilliseconds(100));
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeCloseTo_OutsidePrecision_ShouldPass()
    {
        TimeSpan.FromSeconds(5).Should().NotBeCloseTo(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(1));
    }

    [TestMethod]
    public void NotBeCloseTo_WithinPrecision_ShouldFail()
    {
        Action act = () => TimeSpan.FromSeconds(5).Should().NotBeCloseTo(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(1));
        act.Should().Throw<AssertBoxException>();
    }
}
