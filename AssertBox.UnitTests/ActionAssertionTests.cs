namespace AssertBox.UnitTests;

[TestClass]
public sealed class ActionAssertionTests
{
    [TestMethod]
    public void Func3_ResultShouldBe()
    {
        Func<int, string, uint> func3 = (_, _) => 0;
        func3(1, "abc").Should().Be(0);
    }

    [TestMethod]
    public void Func1_ResultNotSameAs()
    {
        Func<object?> action1 = () => "";
        Func<object?> action2 = () => "";
        action1.Should().NotBeSameAs(action2);
    }

    [TestMethod]
    public void Func1_ResultSameAs()
    {
        Func<object?> action = () => "";
        action.Should().BeSameAs(action);
    }
}