namespace AssertBox.UnitTests;

public static class CustomAssertions
{
    extension(Assertions<int> a)
    {
        public Assertions<int> BeEven()
        {
            Fail.When(
                a.Subject % 2 != 0,
                MessageBuilder.Expected(a.SubjectExpression, "to be even", a.Subject));
            return a;
        }

        public Assertions<int> BeOdd()
        {
            Fail.When(
                a.Subject % 2 == 0,
                MessageBuilder.Expected(a.SubjectExpression, "to be odd", a.Subject));
            return a;
        }
    }
}

[TestClass]
public class ExtensibilityTests
{
    [TestMethod]
    public void CustomAssertion_BeEven_ShouldPass()
    {
        4.Should().BeEven();
    }

    [TestMethod]
    public void CustomAssertion_BeEven_ShouldFail()
    {
        Action act = () => 3.Should().BeEven();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void CustomAssertion_BeOdd_ShouldPass()
    {
        3.Should().BeOdd();
    }

    [TestMethod]
    public void CustomAssertion_CanChainWithBuiltIn()
    {
        4.Should().BeEven().BeGreaterThan(0).BeLessThan(10);
    }

    [TestMethod]
    public void CustomAssertion_UsesPublicToolkit()
    {
        Action act = () => 5.Should().BeEven();
        act.Should().Throw<AssertBoxException>().WithMessage("to be even");
    }

    [TestMethod]
    public void MessageBuilder_Format_HandlesNull()
    {
        MessageBuilder.Format(null).Should().Be("<null>");
    }

    [TestMethod]
    public void MessageBuilder_Format_HandlesStrings()
    {
        MessageBuilder.Format("hello").Should().Be("\"hello\"");
    }

    [TestMethod]
    public void MessageBuilder_Format_HandlesCollections()
    {
        var formatted = MessageBuilder.Format(new[] { 1, 2, 3 });
        formatted.Should().Contain("1").Contain("2").Contain("3");
    }
}
