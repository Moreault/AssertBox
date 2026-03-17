namespace AssertBox.UnitTests;

[TestClass]
public class TimeOnlyAssertionTests
{
    private static readonly TimeOnly Reference = new(10, 30, 45);

    [TestMethod]
    public void BeBefore_WithEarlierTime_ShouldPass()
    {
        new TimeOnly(8, 0).Should().BeBefore(Reference);
    }

    [TestMethod]
    public void BeBefore_WithLaterTime_ShouldFail()
    {
        Action act = () => new TimeOnly(14, 0).Should().BeBefore(Reference);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeAfter_WithLaterTime_ShouldPass()
    {
        new TimeOnly(14, 0).Should().BeAfter(Reference);
    }

    [TestMethod]
    public void BeAfter_WithEarlierTime_ShouldFail()
    {
        Action act = () => new TimeOnly(8, 0).Should().BeAfter(Reference);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeOnOrBefore_WithEqualTime_ShouldPass()
    {
        Reference.Should().BeOnOrBefore(Reference);
    }

    [TestMethod]
    public void BeOnOrAfter_WithEqualTime_ShouldPass()
    {
        Reference.Should().BeOnOrAfter(Reference);
    }

    [TestMethod]
    public void BeCloseTo_WithinPrecision_ShouldPass()
    {
        new TimeOnly(10, 30, 47).Should().BeCloseTo(Reference, TimeSpan.FromSeconds(5));
    }

    [TestMethod]
    public void BeCloseTo_OutsidePrecision_ShouldFail()
    {
        Action act = () => new TimeOnly(11, 30, 45).Should().BeCloseTo(Reference, TimeSpan.FromSeconds(5));
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeCloseTo_OutsidePrecision_ShouldPass()
    {
        new TimeOnly(11, 30, 45).Should().NotBeCloseTo(Reference, TimeSpan.FromSeconds(5));
    }

    [TestMethod]
    public void NotBeCloseTo_WithinPrecision_ShouldFail()
    {
        Action act = () => new TimeOnly(10, 30, 46).Should().NotBeCloseTo(Reference, TimeSpan.FromSeconds(5));
        act.Should().Throw<AssertBoxException>();
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
    public void HaveMillisecond_ShouldPass()
    {
        new TimeOnly(10, 30, 45, 123).Should().HaveMillisecond(123);
    }

    [TestMethod]
    public void HaveMillisecond_WithWrongValue_ShouldFail()
    {
        Action act = () => new TimeOnly(10, 30, 45, 123).Should().HaveMillisecond(999);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Chaining_TimeOnlyAssertions()
    {
        Reference.Should().HaveHour(10).HaveMinute(30).HaveSecond(45);
    }
}
