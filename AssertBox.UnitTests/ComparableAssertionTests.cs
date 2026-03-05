namespace AssertBox.UnitTests;

[TestClass]
public class ComparableAssertionTests
{
    [TestMethod]
    public void BeGreaterThan_WithGreaterValue_ShouldPass()
    {
        10.Should().BeGreaterThan(5);
    }

    [TestMethod]
    public void BeGreaterThan_WithEqualValue_ShouldFail()
    {
        Action act = () => 5.Should().BeGreaterThan(5);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeGreaterThanOrEqualTo_WithEqualValue_ShouldPass()
    {
        5.Should().BeGreaterThanOrEqualTo(5);
    }

    [TestMethod]
    public void BeLessThan_WithLesserValue_ShouldPass()
    {
        5.Should().BeLessThan(10);
    }

    [TestMethod]
    public void BeLessThan_WithGreaterValue_ShouldFail()
    {
        Action act = () => 10.Should().BeLessThan(5);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeLessThanOrEqualTo_WithEqualValue_ShouldPass()
    {
        5.Should().BeLessThanOrEqualTo(5);
    }

    [TestMethod]
    public void BeInRange_WithValueInRange_ShouldPass()
    {
        5.Should().BeInRange(1, 10);
    }

    [TestMethod]
    public void BeInRange_WithValueOutOfRange_ShouldFail()
    {
        Action act = () => 15.Should().BeInRange(1, 10);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Chaining_ComparableAssertions()
    {
        50.Should().BeGreaterThan(0).BeLessThan(100).BeInRange(1, 99);
    }
}
