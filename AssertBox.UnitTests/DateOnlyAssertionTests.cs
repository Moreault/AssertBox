namespace AssertBox.UnitTests;

[TestClass]
public class DateOnlyAssertionTests
{
    private static readonly DateOnly Reference = new(2025, 6, 15);

    [TestMethod]
    public void BeBefore_WithEarlierDate_ShouldPass()
    {
        new DateOnly(2025, 1, 1).Should().BeBefore(Reference);
    }

    [TestMethod]
    public void BeBefore_WithLaterDate_ShouldFail()
    {
        Action act = () => new DateOnly(2026, 1, 1).Should().BeBefore(Reference);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeAfter_WithLaterDate_ShouldPass()
    {
        new DateOnly(2026, 1, 1).Should().BeAfter(Reference);
    }

    [TestMethod]
    public void BeAfter_WithEarlierDate_ShouldFail()
    {
        Action act = () => new DateOnly(2025, 1, 1).Should().BeAfter(Reference);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeOnOrBefore_WithEqualDate_ShouldPass()
    {
        Reference.Should().BeOnOrBefore(Reference);
    }

    [TestMethod]
    public void BeOnOrBefore_WithLaterDate_ShouldFail()
    {
        Action act = () => new DateOnly(2026, 1, 1).Should().BeOnOrBefore(Reference);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeOnOrAfter_WithEqualDate_ShouldPass()
    {
        Reference.Should().BeOnOrAfter(Reference);
    }

    [TestMethod]
    public void BeOnOrAfter_WithEarlierDate_ShouldFail()
    {
        Action act = () => new DateOnly(2025, 1, 1).Should().BeOnOrAfter(Reference);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeCloseTo_WithinPrecision_ShouldPass()
    {
        new DateOnly(2025, 6, 17).Should().BeCloseTo(Reference, 5);
    }

    [TestMethod]
    public void BeCloseTo_OutsidePrecision_ShouldFail()
    {
        Action act = () => new DateOnly(2025, 7, 15).Should().BeCloseTo(Reference, 5);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeCloseTo_OutsidePrecision_ShouldPass()
    {
        new DateOnly(2025, 7, 15).Should().NotBeCloseTo(Reference, 5);
    }

    [TestMethod]
    public void NotBeCloseTo_WithinPrecision_ShouldFail()
    {
        Action act = () => new DateOnly(2025, 6, 16).Should().NotBeCloseTo(Reference, 5);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void HaveYear_ShouldPass()
    {
        Reference.Should().HaveYear(2025);
    }

    [TestMethod]
    public void HaveMonth_ShouldPass()
    {
        Reference.Should().HaveMonth(6);
    }

    [TestMethod]
    public void HaveDay_ShouldPass()
    {
        Reference.Should().HaveDay(15);
    }

    [TestMethod]
    public void Chaining_DateOnlyAssertions()
    {
        Reference.Should().HaveYear(2025).HaveMonth(6).HaveDay(15);
    }
}
