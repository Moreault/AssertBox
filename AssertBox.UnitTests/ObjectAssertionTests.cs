namespace AssertBox.UnitTests;

public record Address(string Street, string City);
public record Person(string Name, int Age, Address? Address = null, List<string>? Tags = null);
public class Node
{
    public string Value { get; set; } = "";
    public Node? Next { get; set; }
}

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
        "hello".Should().BeOfType<string>();
    }

    [TestMethod]
    public void BeOfType_WithNonMatchingType_ShouldFail()
    {
        Action act = () => "hello".Should().BeOfType<int>();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeAssignableTo_WithAssignableType_ShouldPass()
    {
        "hello".Should().BeAssignableTo<IComparable>();
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

    [TestMethod]
    public void BeEquivalentTo_WithSamePropertyValues_ShouldPass()
    {
        var a = new Person("Alice", 30);
        var b = new Person("Alice", 30);
        a.Should().BeEquivalentTo(b);
    }

    [TestMethod]
    public void BeEquivalentTo_WithDifferentPropertyValues_ShouldFail()
    {
        var a = new Person("Alice", 30);
        var b = new Person("Bob", 30);
        Action act = () => a.Should().BeEquivalentTo(b);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeEquivalentTo_WithDifferentPropertyValues_ShouldPass()
    {
        var a = new Person("Alice", 30);
        var b = new Person("Bob", 25);
        a.Should().NotBeEquivalentTo(b);
    }

    [TestMethod]
    public void NotBeEquivalentTo_WithSamePropertyValues_ShouldFail()
    {
        var a = new Person("Alice", 30);
        var b = new Person("Alice", 30);
        Action act = () => a.Should().NotBeEquivalentTo(b);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeEquivalentTo_BothNull_ShouldPass()
    {
        ((Person?)null).Should().BeEquivalentTo(null);
    }

    [TestMethod]
    public void BeEquivalentTo_OneNull_ShouldFail()
    {
        var a = new Person("Alice", 30);
        Action act = () => a.Should().BeEquivalentTo(null!);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeEquivalentTo_WithNestedObjects_Equivalent_ShouldPass()
    {
        var a = new Person("Alice", 30, new Address("123 Main", "Springfield"));
        var b = new Person("Alice", 30, new Address("123 Main", "Springfield"));
        a.Should().BeEquivalentTo(b);
    }

    [TestMethod]
    public void BeEquivalentTo_WithNestedObjects_Different_ShouldFail()
    {
        var a = new Person("Alice", 30, new Address("123 Main", "Springfield"));
        var b = new Person("Alice", 30, new Address("123 Main", "Shelbyville"));
        Action act = () => a.Should().BeEquivalentTo(b);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeEquivalentTo_WithCollectionProperties_Equivalent_ShouldPass()
    {
        var a = new Person("Alice", 30, Tags: new List<string> { "a", "b" });
        var b = new Person("Alice", 30, Tags: new List<string> { "b", "a" });
        a.Should().BeEquivalentTo(b);
    }

    [TestMethod]
    public void BeEquivalentTo_WithCollectionProperties_Different_ShouldFail()
    {
        var a = new Person("Alice", 30, Tags: new List<string> { "a", "b" });
        var b = new Person("Alice", 30, Tags: new List<string> { "a", "c" });
        Action act = () => a.Should().BeEquivalentTo(b);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeEquivalentTo_Primitives_ShouldPass()
    {
        42.Should().BeEquivalentTo(42);
    }

    [TestMethod]
    public void BeEquivalentTo_Primitives_Different_ShouldFail()
    {
        Action act = () => 42.Should().BeEquivalentTo(99);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeEquivalentTo_NullPropertiesOnBothSides_ShouldPass()
    {
        var a = new Person("Alice", 30, Address: null);
        var b = new Person("Alice", 30, Address: null);
        a.Should().BeEquivalentTo(b);
    }

    [TestMethod]
    public void BeEquivalentTo_CircularReferences_ShouldNotStackOverflow()
    {
        var a = new Node { Value = "A" };
        a.Next = a;

        var b = new Node { Value = "A" };
        b.Next = b;

        a.Should().BeEquivalentTo(b);
    }
}
