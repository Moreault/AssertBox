namespace AssertBox.UnitTests;

[TestClass]
public class ExceptionAssertionTests
{
    [TestMethod]
    public void Throw_WithMatchingException_ShouldPass()
    {
        Action act = () => throw new InvalidOperationException("test");
        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void Throw_WithNoException_ShouldFail()
    {
        Action act = () => { };
        Action test = () => act.Should().Throw<InvalidOperationException>();
        test.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Throw_WithWrongException_ShouldFail()
    {
        Action act = () => throw new ArgumentException("test");
        Action test = () => act.Should().Throw<InvalidOperationException>();
        test.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotThrow_WithNoException_ShouldPass()
    {
        Action act = () => { };
        act.Should().NotThrow();
    }

    [TestMethod]
    public void NotThrow_WithException_ShouldFail()
    {
        Action act = () => throw new Exception("boom");
        Action test = () => act.Should().NotThrow();
        test.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Throw_WithMessage_ShouldPass()
    {
        Action act = () => throw new InvalidOperationException("something went wrong");
        act.Should().Throw<InvalidOperationException>().WithMessage("went wrong");
    }

    [TestMethod]
    public void Throw_WithMessage_ShouldFail()
    {
        Action act = () => throw new InvalidOperationException("something went wrong");
        Action test = () => act.Should().Throw<InvalidOperationException>().WithMessage("not found");
        test.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Throw_WithInnerException_ShouldPass()
    {
        Action act = () => throw new InvalidOperationException("outer", new ArgumentException("inner"));
        act.Should().Throw<InvalidOperationException>()
            .WithInnerException<InvalidOperationException, ArgumentException>();
    }

    [TestMethod]
    public void Throw_WithParameterName_ShouldPass()
    {
        Action act = () => throw new ArgumentException("bad value", "myParam");
        act.Should().Throw<ArgumentException>().WithParameterName("myParam");
    }

    [TestMethod]
    public void Throw_WithParameterName_WrongName_ShouldFail()
    {
        Action act = () => throw new ArgumentException("bad value", "myParam");
        Action test = () => act.Should().Throw<ArgumentException>().WithParameterName("otherParam");
        test.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Throw_WithParameterName_Chained_ShouldPass()
    {
        Action act = () => throw new ArgumentException("bad value", "myParam");
        act.Should().Throw<ArgumentException>()
            .WithParameterName("myParam")
            .WithMessage("bad value");
    }

    [TestMethod]
    public void FuncT_Throw_WithMatchingException_ShouldPass()
    {
        Func<int> act = () => throw new InvalidOperationException("test");
        act.Should().Throw<InvalidOperationException>();
    }

    [TestMethod]
    public void FuncT_Throw_WithNoException_ShouldFail()
    {
        Func<int> act = () => 42;
        Action test = () => act.Should().Throw<InvalidOperationException>();
        test.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void FuncT_Throw_WithWrongException_ShouldFail()
    {
        Func<int> act = () => throw new ArgumentException("test");
        Action test = () => act.Should().Throw<InvalidOperationException>();
        test.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void FuncT_NotThrow_WithNoException_ShouldPass()
    {
        Func<int> act = () => 42;
        act.Should().NotThrow();
    }

    [TestMethod]
    public void FuncT_NotThrow_WithException_ShouldFail()
    {
        Func<int> act = () => throw new Exception("boom");
        Action test = () => act.Should().NotThrow();
        test.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void FuncT_Throw_WithMessage_ShouldPass()
    {
        Func<string> act = () => throw new InvalidOperationException("something went wrong");
        act.Should().Throw<InvalidOperationException>().WithMessage("went wrong");
    }

    [TestMethod]
    public void FuncT_Throw_WithParameterName_ShouldPass()
    {
        Func<int> act = () => throw new ArgumentException("bad value", "myParam");
        act.Should().Throw<ArgumentException>()
            .WithParameterName("myParam")
            .WithMessage("bad value");
    }

    [TestMethod]
    public async Task ThrowAsync_WithMatchingException_ShouldPass()
    {
        Func<Task> act = () => throw new InvalidOperationException("async test");
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [TestMethod]
    public async Task ThrowAsync_WithNoException_ShouldFail()
    {
        Func<Task> act = () => Task.CompletedTask;
        Func<Task> test = async () => await act.Should().ThrowAsync<InvalidOperationException>();
        await test.Should().ThrowAsync<AssertBoxException>();
    }

    [TestMethod]
    public async Task NotThrowAsync_WithNoException_ShouldPass()
    {
        Func<Task> act = () => Task.CompletedTask;
        await act.Should().NotThrowAsync();
    }
}
