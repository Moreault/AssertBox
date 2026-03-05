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
        new[] { 1, 2, 3 }.Should().Contain((Func<int, bool>)(x => x > 2));
    }

    [TestMethod]
    public void AllSatisfy_WithAllMatching_ShouldPass()
    {
        new[] { 2, 4, 6 }.Should().AllSatisfy((Func<int, bool>)(x => x % 2 == 0));
    }

    [TestMethod]
    public void AllSatisfy_WithSomeNotMatching_ShouldFail()
    {
        Action act = () => new[] { 2, 3, 6 }.Should().AllSatisfy((Func<int, bool>)(x => x % 2 == 0));
        act.Should().Throw<AssertBoxException>();
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
        new[] { 2, 4, 6 }.Should().OnlyContain((Func<int, bool>)(x => x % 2 == 0));
    }

    [TestMethod]
    public void WorksWithList()
    {
        new List<string> { "a", "b" }.Should().HaveCount(2).Contain("a");
    }
}
