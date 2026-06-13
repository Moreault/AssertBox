using System.Collections.Immutable;

namespace AssertBox.UnitTests;

[TestClass]
public class CollectionAssertionAdditionsTests
{
    private sealed record Garbage
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
    }

    [TestMethod]
    public void OnlyHaveUniqueItems_WithAllUnique_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().OnlyHaveUniqueItems();
    }

    [TestMethod]
    public void OnlyHaveUniqueItems_WithDuplicate_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 2, 3 }.Should().OnlyHaveUniqueItems();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void OnlyHaveUniqueItems_WithEmptyCollection_ShouldPass()
    {
        Array.Empty<int>().Should().OnlyHaveUniqueItems();
    }

    [TestMethod]
    public void OnlyHaveUniqueItems_OnReadOnlyList_ShouldPass()
    {
        IReadOnlyList<int> subject = new List<int> { 4, 5, 6 };
        subject.Should().OnlyHaveUniqueItems();
    }

    [TestMethod]
    public void OnlyHaveUniqueItems_WithDuplicateReferenceTypes_ShouldFail()
    {
        var duplicate = new Garbage { Id = 1, Name = "Bob" };
        Action act = () => new List<Garbage> { duplicate, duplicate }.Should().OnlyHaveUniqueItems();
        act.Should().Throw<AssertBoxException>();
    }

    //ContainInOrder with a collection argument

    [TestMethod]
    public void ContainInOrder_WithCollectionInOrder_ShouldPass()
    {
        new[] { 1, 2, 3, 4, 5 }.Should().ContainInOrder(new List<int> { 2, 4 });
    }

    [TestMethod]
    public void ContainInOrder_WithCollectionOutOfOrder_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3, 4, 5 }.Should().ContainInOrder(new List<int> { 4, 2 });
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void ContainInOrder_WithFullCollectionInSameOrder_ShouldPass()
    {
        var first = new Garbage { Id = 1, Name = "A" };
        var second = new Garbage { Id = 2, Name = "B" };
        var third = new Garbage { Id = 3, Name = "C" };

        new[] { first, second, third }.Should().ContainInOrder(new List<Garbage> { first, second, third });
    }

    [TestMethod]
    public void ContainInOrder_WithReferenceTypeCollectionOnListReceiver_ShouldPass()
    {
        var first = new Garbage { Id = 1, Name = "A" };
        var second = new Garbage { Id = 2, Name = "B" };

        new List<Garbage> { first, second }.Should().ContainInOrder(new List<Garbage> { first, second });
    }

    //NotContainInOrder

    [TestMethod]
    public void NotContainInOrder_WithDifferentOrder_ShouldPass()
    {
        var first = new Garbage { Id = 1, Name = "A" };
        var second = new Garbage { Id = 2, Name = "B" };
        var third = new Garbage { Id = 3, Name = "C" };

        new[] { third, first, second }.Should().NotContainInOrder(new List<Garbage> { first, second, third });
    }

    [TestMethod]
    public void NotContainInOrder_WhenSubjectContainsExpectedInOrder_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3, 4, 5 }.Should().NotContainInOrder(new List<int> { 2, 4 });
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotContainInOrder_WithParams_WhenNotInOrder_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().NotContainInOrder(3, 1);
    }

    [TestMethod]
    public void NotContainInOrder_WithEmptyExpected_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().NotContainInOrder(new List<int>());
    }

    [TestMethod]
    public void NotContainInOrder_OnGenericConstrainedCollection_ShouldPass()
    {
        AssertNotInOrder(new List<Garbage> { new() { Id = 3 }, new() { Id = 1 }, new() { Id = 2 } },
            new List<Garbage> { new() { Id = 1 }, new() { Id = 2 }, new() { Id = 3 } });
    }

    private static void AssertNotInOrder<TCollection>(TCollection subject, IEnumerable<Garbage> unexpected)
        where TCollection : class, IEnumerable<Garbage>
    {
        subject.Should().NotContainInOrder(unexpected);
    }

    [TestMethod]
    public void NotContain_WithPredicateNoMatch_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().NotContain(x => x > 10);
    }

    [TestMethod]
    public void NotContain_WithPredicateMatch_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().NotContain(x => x == 2);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotContain_WithPredicateOnList_ShouldPass()
    {
        new List<Garbage> { new() { Name = "Roger" } }.Should().NotContain(x => x.Name == "Seb");
    }

    [TestMethod]
    public void NotContain_WithPredicateOnIList_ShouldFail()
    {
        IList<Garbage> subject = new List<Garbage> { new() { Name = "Roger" } };
        Action act = () => subject.Should().NotContain(x => x.Name == "Roger");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotContain_WithPredicateOnReadOnlyList_ShouldPass()
    {
        IReadOnlyList<Garbage> subject = new List<Garbage> { new() { Name = "Roger" } };
        subject.Should().NotContain(x => x.Name == "Seb");
    }

    [TestMethod]
    public void NotContain_WithPredicateOnImmutableList_ShouldPass()
    {
        ImmutableList.Create(1, 2, 3).Should().NotContain(x => x > 10);
    }

    //Dictionary predicate overloads

    [TestMethod]
    public void Contain_WithDictionaryPredicate_ShouldPass()
    {
        var subject = new Dictionary<string, Garbage> { ["a"] = new() { Name = "Roger" } };
        subject.Should().Contain(x => x.Value.Name == "Roger");
    }

    [TestMethod]
    public void NotContain_WithDictionaryPredicateNoMatch_ShouldPass()
    {
        var subject = new Dictionary<string, Garbage> { ["a"] = new() { Name = "Roger" } };
        subject.Should().NotContain(x => x.Value.Name == "Seb");
    }

    [TestMethod]
    public void NotContain_WithDictionaryPredicateMatch_ShouldFail()
    {
        var subject = new Dictionary<string, Garbage> { ["a"] = new() { Name = "Roger" } };
        Action act = () => subject.Should().NotContain(x => x.Value.Name == "Roger");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotContain_WithIntKeyedDictionaryPredicateMatch_ShouldFail()
    {
        var subject = new Dictionary<int, Garbage> { [1] = new() { Name = "Roger" } };
        Action act = () => subject.Should().NotContain(x => x.Value.Name == "Roger");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Chaining_WithNewMembers_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().OnlyHaveUniqueItems().NotContain(x => x > 10).ContainInOrder(new List<int> { 1, 3 });
    }
}
