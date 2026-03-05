namespace AssertBox.UnitTests;

[TestClass]
public class NumericAssertionTests
{
    [TestMethod]
    public void BePositive_WithPositive_ShouldPass()
    {
        42.Should().BePositive();
    }

    [TestMethod]
    public void BePositive_WithZero_ShouldFail()
    {
        Action act = () => 0.Should().BePositive();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BePositive_WithNegative_ShouldFail()
    {
        Action act = () => (-1).Should().BePositive();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeNegative_WithNegative_ShouldPass()
    {
        (-5).Should().BeNegative();
    }

    [TestMethod]
    public void BeNegative_WithPositive_ShouldFail()
    {
        Action act = () => 5.Should().BeNegative();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeZero_WithZero_ShouldPass()
    {
        0.Should().BeZero();
    }

    [TestMethod]
    public void BeZero_WithNonZero_ShouldFail()
    {
        Action act = () => 1.Should().BeZero();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeApproximately_WithinPrecision_ShouldPass()
    {
        3.14.Should().BeApproximately(3.15, 0.02);
    }

    [TestMethod]
    public void BeApproximately_OutsidePrecision_ShouldFail()
    {
        Action act = () => 3.14.Should().BeApproximately(3.20, 0.01);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void GenericMath_WorksWithFloat()
    {
        3.14f.Should().BePositive().BeApproximately(3.14f, 0.001f);
    }

    [TestMethod]
    public void GenericMath_WorksWithDecimal()
    {
        100m.Should().BePositive();
    }
}
