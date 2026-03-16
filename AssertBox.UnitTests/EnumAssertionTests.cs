namespace AssertBox.UnitTests;

[TestClass]
public class EnumAssertionTests
{
    private enum Color
    {
        Red,
        Green,
        Blue
    }

    [Flags]
    private enum Permissions
    {
        None = 0,
        Read = 1,
        Write = 2,
        Execute = 4
    }

    [TestMethod]
    public void BeDefined_WithDefinedValue_ShouldPass()
    {
        Color.Red.Should().BeDefined();
    }

    [TestMethod]
    public void BeDefined_WithUndefinedValue_ShouldFail()
    {
        Action act = () => ((Color)99).Should().BeDefined();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeDefined_WithUndefinedValue_ShouldPass()
    {
        ((Color)99).Should().NotBeDefined();
    }

    [TestMethod]
    public void NotBeDefined_WithDefinedValue_ShouldFail()
    {
        Action act = () => Color.Green.Should().NotBeDefined();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void HaveFlag_WithPresentFlag_ShouldPass()
    {
        (Permissions.Read | Permissions.Write).Should().HaveFlag(Permissions.Read);
    }

    [TestMethod]
    public void HaveFlag_WithMissingFlag_ShouldFail()
    {
        Action act = () => Permissions.Read.Should().HaveFlag(Permissions.Write);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotHaveFlag_WithMissingFlag_ShouldPass()
    {
        Permissions.Read.Should().NotHaveFlag(Permissions.Execute);
    }

    [TestMethod]
    public void NotHaveFlag_WithPresentFlag_ShouldFail()
    {
        Action act = () => (Permissions.Read | Permissions.Write).Should().NotHaveFlag(Permissions.Read);
        act.Should().Throw<AssertBoxException>();
    }
}
