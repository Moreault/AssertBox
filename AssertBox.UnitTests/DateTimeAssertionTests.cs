namespace AssertBox.UnitTests;

[TestClass]
public class DateTimeAssertionTests
{
    private static readonly DateTime Reference = new(2025, 6, 15, 10, 30, 45);

    [TestMethod]
    public void BeBefore_WithEarlierDate_ShouldPass()
    {
        new DateTime(2025, 1, 1).Should().BeBefore(Reference);
    }

    [TestMethod]
    public void BeBefore_WithLaterDate_ShouldFail()
    {
        Action act = () => new DateTime(2026, 1, 1).Should().BeBefore(Reference);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeAfter_WithLaterDate_ShouldPass()
    {
        new DateTime(2026, 1, 1).Should().BeAfter(Reference);
    }

    [TestMethod]
    public void BeAfter_WithEarlierDate_ShouldFail()
    {
        Action act = () => new DateTime(2024, 1, 1).Should().BeAfter(Reference);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeOnOrBefore_WithEqualDate_ShouldPass()
    {
        Reference.Should().BeOnOrBefore(Reference);
    }

    [TestMethod]
    public void BeOnOrAfter_WithEqualDate_ShouldPass()
    {
        Reference.Should().BeOnOrAfter(Reference);
    }

    [TestMethod]
    public void BeCloseTo_WithinPrecision_ShouldPass()
    {
        var close = Reference.AddSeconds(2);
        close.Should().BeCloseTo(Reference, TimeSpan.FromSeconds(5));
    }

    [TestMethod]
    public void BeCloseTo_OutsidePrecision_ShouldFail()
    {
        var far = Reference.AddHours(1);
        Action act = () => far.Should().BeCloseTo(Reference, TimeSpan.FromSeconds(5));
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
    public void HaveHour_ShouldPass()
    {
        Reference.Should().HaveHour(10);
    }

    [TestMethod]
    public void HaveMinute_ShouldPass()
    {
        Reference.Should().HaveMinute(30);
    }

    [TestMethod]
    public void HaveSecond_ShouldPass()
    {
        Reference.Should().HaveSecond(45);
    }

    [TestMethod]
    public void Chaining_DateTimeAssertions()
    {
        Reference.Should().HaveYear(2025).HaveMonth(6).HaveDay(15);
    }
}
