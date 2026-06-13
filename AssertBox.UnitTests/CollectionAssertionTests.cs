using System.Collections.Immutable;

namespace AssertBox.UnitTests;

[TestClass]
public class CollectionAssertionTests
{
    [TestMethod]
    public void BeEmpty_WithEmptyCollection_ShouldPass()
    {
        Array.Empty<int>().Should().BeEmpty();
    }

    [TestMethod]
    public void BeEmpty_WithNonEmptyCollection_ShouldFail()
    {
        Action act = () => new[] { 1 }.Should().BeEmpty();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeEmpty_WithNonEmptyCollection_ShouldPass()
    {
        new[] { 1 }.Should().NotBeEmpty();
    }

    [TestMethod]
    public void HaveCount_WithCorrectCount_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().HaveCount(3);
    }

    [TestMethod]
    public void HaveCount_WithIncorrectCount_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().HaveCount(5);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Contain_WithPresentElement_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().Contain(2);
    }

    [TestMethod]
    public void Contain_WithMissingElement_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().Contain(99);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotContain_WithMissingElement_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().NotContain(99);
    }

    [TestMethod]
    public void Contain_WithPredicate_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().Contain(x => x > 2);
    }

    [TestMethod]
    public void AllSatisfy_WithAllMatching_ShouldPass()
    {
        new[] { 2, 4, 6 }.Should().AllSatisfy(x => x % 2 == 0);
    }

    [TestMethod]
    public void AllSatisfy_WithSomeNotMatching_ShouldFail()
    {
        Action act = () => new[] { 2, 3, 6 }.Should().AllSatisfy(x => x % 2 == 0);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void AllSatisfy_WithSameElement_ShouldPass()
    {
        var stuff = new[] { 1, 1, 1 };
        stuff.Should().AllSatisfy(x => x.Should().Be(1));
    }

    [TestMethod]
    public void BeEquivalentTo_SameElementsDifferentOrder_ShouldPass()
    {
        new[] { 3, 1, 2 }.Should().BeEquivalentTo([1, 2, 3]);
    }

    [TestMethod]
    public void BeEquivalentTo_DifferentElements_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().BeEquivalentTo([1, 2, 4]);
        act.Should().Throw<AssertBoxException>();
    }

    public sealed record Garbage
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }

    [TestMethod]
    public void BeEquivalentTo_SameNonComparableElements_ShouldPass()
    {
        IEnumerable<Garbage> list1 = new List<Garbage>
        {
            new() { Id = 44, Name = "Roger" },
            new() { Id = 45, Name = "Seb" },
            new() { Id = 86, Name = "Gertrude" },
        };

        IEnumerable<Garbage> list2 = new List<Garbage>
        {
            new() { Id = 44, Name = "Roger" },
            new() { Id = 45, Name = "Seb" },
            new() { Id = 86, Name = "Gertrude" },
        };

        list1.Should().BeEquivalentTo(list2);
    }

    [TestMethod]
    public void ContainInOrder_WithCorrectOrder_ShouldPass()
    {
        new[] { 1, 2, 3, 4, 5 }.Should().ContainInOrder(2, 4);
    }

    [TestMethod]
    public void ContainInOrder_WithWrongOrder_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3, 4, 5 }.Should().ContainInOrder(4, 2);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeInAscendingOrder_WithSortedCollection_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().BeInAscendingOrder<int[], int>();
    }

    [TestMethod]
    public void BeInAscendingOrder_WithUnsortedCollection_ShouldFail()
    {
        Action act = () => new[] { 3, 1, 2 }.Should().BeInAscendingOrder<int[], int>();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeInDescendingOrder_WithSortedCollection_ShouldPass()
    {
        new[] { 3, 2, 1 }.Should().BeInDescendingOrder<int[], int>();
    }

    [TestMethod]
    public void HaveCountGreaterThan_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().HaveCountGreaterThan(2);
    }

    [TestMethod]
    public void HaveCountLessThan_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().HaveCountLessThan(5);
    }

    [TestMethod]
    public void Chaining_CollectionAssertions()
    {
        new[] { 1, 2, 3 }.Should().NotBeEmpty().HaveCount(3).Contain(2);
    }

    [TestMethod]
    public void OnlyContain_WithAllMatching_ShouldPass()
    {
        new[] { 2, 4, 6 }.Should().OnlyContain(x => x % 2 == 0);
    }

    [TestMethod]
    public void WorksWithList()
    {
        new List<string> { "a", "b" }.Should().HaveCount(2).Contain("a");
    }

    [TestMethod]
    public void OnlyContain_WithListAndLambda_ShouldInferTypes()
    {
        new List<string> { "a", "bb" }.Should().OnlyContain(x => x.Length > 0);
    }

    [TestMethod]
    public void OnlyContain_WithImmutableListAndLambda_ShouldPass()
    {
        var abc = ImmutableList.CreateRange(new List<(int Id, string Name)>
        {
            (44, "Roger"),
            (44, "Roger"),
            (44, "Roger"),
        });
        abc.Should().OnlyContain(x => x == (Id: 44, Name: "Roger"));
    }

    [TestMethod]
    public void Contain_WithCollectionAllPresent_ShouldPass()
    {
        new[] { 1, 2, 3, 4 }.Should().Contain(new List<int> { 2, 4 });
    }

    [TestMethod]
    public void Contain_WithCollectionSomeMissing_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().Contain(new List<int> { 2, 99 });
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Contain_WithProjectedEnumerableAndCollection_ShouldPass()
    {
        IEnumerable<string> names = new[] { "a", "b", "c" }.Select(x => x);
        names.Should().Contain(new List<string> { "a", "c" });
    }

    [TestMethod]
    public void NotContain_WithCollectionNonePresent_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().NotContain(new List<int> { 7, 8 });
    }

    [TestMethod]
    public void NotContain_WithCollectionSomePresent_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().NotContain(new List<int> { 3, 8 });
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Contain_WithSingleStringElement_ShouldResolveToElementOverload()
    {
        new List<string> { "ab", "cd" }.Should().Contain("ab");
    }

    [TestMethod]
    public void Contain_WithSingleStringElementMissing_ShouldFail()
    {
        Action act = () => new List<string> { "ab", "cd" }.Should().Contain("zz");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Contain_WithTypeCollection_ShouldPass()
    {
        new List<Type> { typeof(int), typeof(string), typeof(bool) }
            .Should().Contain(new List<Type> { typeof(string), typeof(bool) });
    }

    [TestMethod]
    public void NotContain_WithTypeCollection_ShouldPass()
    {
        new List<Type> { typeof(int), typeof(string) }
            .Should().NotContain(new List<Type> { typeof(bool), typeof(decimal) });
    }

    private sealed class Wrapper(int value)
    {
        public int Value { get; } = value;

        public override bool Equals(object? obj) => obj switch
        {
            Wrapper w => w.Value == Value,
            int i => i == Value,
            _ => false
        };

        public override int GetHashCode() => Value.GetHashCode();
    }

    [TestMethod]
    public void BeEquivalentTo_WithDifferentElementTypesMatchedByEquals_ShouldPass()
    {
        IReadOnlyList<Wrapper> subject = new List<Wrapper> { new(1), new(2), new(3) };
        IEnumerable<int> expected = new List<int> { 3, 2, 1 };
        subject.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public void BeEquivalentTo_WithDifferentElementTypesNotMatching_ShouldFail()
    {
        IReadOnlyList<Wrapper> subject = new List<Wrapper> { new(1), new(2) };
        IEnumerable<int> expected = new List<int> { 1, 9 };
        Action act = () => subject.Should().BeEquivalentTo(expected);
        act.Should().Throw<AssertBoxException>();
    }

    private sealed class ThrowingGetter
    {
        public int Safe { get; init; }
        public bool Explode { get; init; }
        public int Boom => Explode ? throw new InvalidOperationException("getter blew up") : 0;
    }

    [TestMethod]
    public void BeEquivalentTo_WhenPropertyGetterThrowsTheSameWayOnBothSides_ShouldTreatAsEquivalent()
    {
        var subject = new[] { new ThrowingGetter { Safe = 1, Explode = true } };
        var expected = new List<ThrowingGetter> { new() { Safe = 1, Explode = true } };
        Action act = () => subject.Should().BeEquivalentTo(expected);
        act.Should().NotThrow();
    }

    [TestMethod]
    public void BeEquivalentTo_WhenPropertyGetterThrowsOnOnlyOneSide_ShouldFail()
    {
        var subject = new[] { new ThrowingGetter { Safe = 1, Explode = true } };
        var expected = new List<ThrowingGetter> { new() { Safe = 1, Explode = false } };
        Action act = () => subject.Should().BeEquivalentTo(expected);
        act.Should().Throw<AssertBoxException>();
    }
}
