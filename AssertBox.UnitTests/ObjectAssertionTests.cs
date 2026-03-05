namespace AssertBox.UnitTests;

[TestClass]
public class ObjectAssertionTests
{
    [TestMethod]
    public void Be_WithEqualValues_ShouldPass()
    {
        42.Should().Be(42);
    }

    [TestMethod]
    public void Be_WithDifferentValues_ShouldFail()
    {
        Action act = () => 42.Should().Be(99);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBe_WithDifferentValues_ShouldPass()
    {
        42.Should().NotBe(99);
    }

    [TestMethod]
    public void NotBe_WithEqualValues_ShouldFail()
    {
        Action act = () => 42.Should().NotBe(42);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeNull_WithNull_ShouldPass()
    {
        ((string?)null).Should().BeNull();
    }

    [TestMethod]
    public void BeNull_WithNonNull_ShouldFail()
    {
        Action act = () => "hello".Should().BeNull();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeNull_WithNonNull_ShouldPass()
    {
        "hello".Should().NotBeNull();
    }

    [TestMethod]
    public void NotBeNull_WithNull_ShouldFail()
    {
        Action act = () => ((string?)null).Should().NotBeNull();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeSameAs_WithSameReference_ShouldPass()
    {
        var obj = new object();
        obj.Should().BeSameAs(obj);
    }

    [TestMethod]
    public void BeSameAs_WithDifferentReference_ShouldFail()
    {
        Action act = () => new object().Should().BeSameAs(new object());
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeSameAs_WithDifferentReference_ShouldPass()
    {
        new object().Should().NotBeSameAs(new object());
    }

    [TestMethod]
    public void BeOfType_WithMatchingType_ShouldPass()
    {
        "hello".Should().BeOfType<string, string>();
    }

    [TestMethod]
    public void BeOfType_WithNonMatchingType_ShouldFail()
    {
        Action act = () => "hello".Should().BeOfType<int, string>();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeAssignableTo_WithAssignableType_ShouldPass()
    {
        "hello".Should().BeAssignableTo<IComparable, string>();
    }

    [TestMethod]
    public void Satisfy_WithPassingPredicate_ShouldPass()
    {
        42.Should().Satisfy(x => x > 0);
    }

    [TestMethod]
    public void Satisfy_WithFailingPredicate_ShouldFail()
    {
        Action act = () => 42.Should().Satisfy(x => x < 0);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Chaining_MultipleAssertions_ShouldWork()
    {
        42.Should().NotBeNull().Be(42).NotBe(0);
    }
}
