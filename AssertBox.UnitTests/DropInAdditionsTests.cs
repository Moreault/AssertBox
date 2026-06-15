namespace AssertBox.UnitTests;

[TestClass]
public class DropInAdditionsTests
{
    private interface IThing;
    private sealed record Thing(int Value) : IThing;

    [TestMethod]
    public void And_AllowsFluentAssertionsStyleChaining()
    {
        object value = "hello";
        value.Should().NotBeNull().And.BeOfType<string>().And.BeAssignableTo<IComparable>();
    }

    [TestMethod]
    public void NotBeOfType_WithDifferentType_ShouldPass()
    {
        "hello".Should().NotBeOfType<int>();
    }

    [TestMethod]
    public void NotBeOfType_WithMatchingType_ShouldFail()
    {
        Action act = () => "hello".Should().NotBeOfType<string>();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeOfTypeNonGeneric_WithMatchingType_ShouldPass()
    {
        "hello".Should().BeOfType(typeof(string));
    }

    [TestMethod]
    public void BeOfTypeNonGeneric_WithNonMatchingType_ShouldFail()
    {
        Action act = () => "hello".Should().BeOfType(typeof(int));
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeAssignableToNonGeneric_WithAssignableType_ShouldPass()
    {
        new Thing(1).Should().BeAssignableTo(typeof(IThing));
    }

    [TestMethod]
    public void NotBeAssignableTo_WithUnassignableType_ShouldPass()
    {
        "hello".Should().NotBeAssignableTo<IThing>();
    }

    [TestMethod]
    public void NotBeAssignableTo_WithAssignableType_ShouldFail()
    {
        Action act = () => ((IThing)new Thing(1)).Should().NotBeAssignableTo<IThing>();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void HaveValue_WithValue_ShouldPass()
    {
        ((int?)5).Should().HaveValue();
    }

    [TestMethod]
    public void HaveValue_WithNull_ShouldFail()
    {
        Action act = () => ((int?)null).Should().HaveValue();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotHaveValue_WithNull_ShouldPass()
    {
        ((int?)null).Should().NotHaveValue();
    }

    [TestMethod]
    public void NotHaveValue_WithValue_ShouldFail()
    {
        Action act = () => ((int?)5).Should().NotHaveValue();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotStartWith_WithDifferentPrefix_ShouldPass()
    {
        "hello".Should().NotStartWith("xyz");
    }

    [TestMethod]
    public void NotStartWith_WithMatchingPrefix_ShouldFail()
    {
        Action act = () => "hello".Should().NotStartWith("he");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotEndWith_WithDifferentSuffix_ShouldPass()
    {
        "hello".Should().NotEndWith("xyz");
    }

    [TestMethod]
    public void NotEndWith_WithMatchingSuffix_ShouldFail()
    {
        Action act = () => "hello".Should().NotEndWith("lo");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotMatch_WithNonMatchingPattern_ShouldPass()
    {
        "hello".Should().NotMatch(@"^\d+$");
    }

    [TestMethod]
    public void NotMatch_WithMatchingPattern_ShouldFail()
    {
        Action act = () => "hello".Should().NotMatch(@"^\w+$");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void ContainAll_WhenAllPresent_ShouldPass()
    {
        "the quick brown fox".Should().ContainAll("quick", "fox");
    }

    [TestMethod]
    public void ContainAll_WhenSomeMissing_ShouldFail()
    {
        Action act = () => "the quick brown fox".Should().ContainAll("quick", "dog");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void ContainAny_WhenAtLeastOnePresent_ShouldPass()
    {
        "the quick brown fox".Should().ContainAny("dog", "fox");
    }

    [TestMethod]
    public void ContainAny_WhenNonePresent_ShouldFail()
    {
        Action act = () => "the quick brown fox".Should().ContainAny("dog", "cat");
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeApproximately_WhenFarApart_ShouldPass()
    {
        10.0.Should().NotBeApproximately(20.0, 1.0);
    }

    [TestMethod]
    public void NotBeApproximately_WhenWithinPrecision_ShouldFail()
    {
        Action act = () => 10.0.Should().NotBeApproximately(10.5, 1.0);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void HaveCountGreaterThanOrEqualTo_WhenEqual_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().HaveCountGreaterThanOrEqualTo(3);
    }

    [TestMethod]
    public void HaveCountLessThanOrEqualTo_WhenEqual_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().HaveCountLessThanOrEqualTo(3);
    }

    [TestMethod]
    public void NotHaveCount_WithDifferentCount_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().NotHaveCount(2);
    }

    [TestMethod]
    public void NotHaveCount_WithSameCount_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().NotHaveCount(3);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void HaveSameCount_WhenSame_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().HaveSameCount(new[] { 4, 5, 6 });
    }

    [TestMethod]
    public void HaveSameCount_WhenDifferent_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().HaveSameCount(new[] { 4, 5 });
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotHaveSameCount_WhenDifferent_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().NotHaveSameCount(new[] { 4, 5 });
    }

    [TestMethod]
    public void Equal_WithSameSequence_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().Equal(1, 2, 3);
    }

    [TestMethod]
    public void Equal_WithDifferentOrder_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().Equal(3, 2, 1);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void Equal_WithDifferentLength_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().Equal(1, 2);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotEqual_WithDifferentSequence_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().NotEqual([3, 2, 1]);
    }

    [TestMethod]
    public void NotEqual_WithSameSequence_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().NotEqual([1, 2, 3]);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void ContainSingle_WithOneElement_ShouldPass()
    {
        new[] { 42 }.Should().ContainSingle();
    }

    [TestMethod]
    public void ContainSingle_WithManyElements_ShouldFail()
    {
        Action act = () => new[] { 1, 2 }.Should().ContainSingle();
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void ContainSinglePredicate_WithOneMatch_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().ContainSingle(x => x == 2);
    }

    [TestMethod]
    public void ContainSinglePredicate_WithMultipleMatches_ShouldFail()
    {
        Action act = () => new[] { 2, 2, 3 }.Should().ContainSingle(x => x == 2);
        act.Should().Throw<AssertBoxException>();
    }

    // --- HaveElementAt ---

    [TestMethod]
    public void HaveElementAt_WithMatchingElement_ShouldPass()
    {
        new[] { 1, 2, 3 }.Should().HaveElementAt(1, 2);
    }

    [TestMethod]
    public void HaveElementAt_WithWrongElement_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().HaveElementAt(1, 99);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void HaveElementAt_WithOutOfRangeIndex_ShouldFail()
    {
        Action act = () => new[] { 1, 2, 3 }.Should().HaveElementAt(5, 1);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void BeSubsetOf_WhenSubset_ShouldPass()
    {
        new[] { 1, 2 }.Should().BeSubsetOf([1, 2, 3]);
    }

    [TestMethod]
    public void BeSubsetOf_WhenNotSubset_ShouldFail()
    {
        Action act = () => new[] { 1, 4 }.Should().BeSubsetOf([1, 2, 3]);
        act.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void NotBeSubsetOf_WhenNotSubset_ShouldPass()
    {
        new[] { 1, 4 }.Should().NotBeSubsetOf([1, 2, 3]);
    }

    [TestMethod]
    public void Where_WhenPredicateMatches_ShouldPass()
    {
        Action act = () => throw new InvalidOperationException("boom");
        act.Should().Throw<InvalidOperationException>().Where(x => x.Message.Contains("boom"));
    }

    [TestMethod]
    public void Where_WhenPredicateFails_ShouldThrow()
    {
        Action act = () => throw new InvalidOperationException("boom");
        Action assertion = () => act.Should().Throw<InvalidOperationException>().Where(x => x.Message.Contains("nope"));
        assertion.Should().Throw<AssertBoxException>();
    }

    [TestMethod]
    public void WithInnerExceptionExactly_WhenExactMatch_ShouldPass()
    {
        Action act = () => throw new InvalidOperationException("boom", new ArgumentException("inner"));
        act.Should().Throw<InvalidOperationException>().WithInnerExceptionExactly<InvalidOperationException, ArgumentException>();
    }

    [TestMethod]
    public void WithInnerExceptionExactly_WhenDerivedType_ShouldFail()
    {
        Action act = () => throw new InvalidOperationException("boom", new ArgumentNullException("inner"));
        Action assertion = () => act.Should().Throw<InvalidOperationException>().WithInnerExceptionExactly<InvalidOperationException, ArgumentException>();
        assertion.Should().Throw<AssertBoxException>();
    }

    private sealed record Wrapper(int Id, string Name);

    [TestMethod]
    public void BeEquivalentTo_OnDifference_MessageShowsStackedExpectedAndActual()
    {
        var actual = new Wrapper(1, "Roger");
        var expected = new Wrapper(2, "Roger");

        var message = CaptureMessage(() => actual.Should().BeEquivalentTo(expected));

        message.Should().Contain("difference at 'Id'");
        message.Should().Contain("Expected: 2");
        message.Should().Contain("Actual:   1");
    }

    [TestMethod]
    public void BeEquivalentTo_OnNestedDifference_MessageReportsNestedPathAndValues()
    {
        var actual = new Wrapper(1, "Roger");
        var expected = new Wrapper(1, "Bob");

        var message = CaptureMessage(() => actual.Should().BeEquivalentTo(expected));

        message.Should().Contain("difference at 'Name'");
        message.Should().Contain("Expected: \"Bob\"");
        message.Should().Contain("Actual:   \"Roger\"");
    }

    private static string CaptureMessage(Action action)
    {
        try
        {
            action();
        }
        catch (AssertBoxException ex)
        {
            return ex.Message;
        }
        throw new InvalidOperationException("Expected an AssertBoxException but none was thrown.");
    }
}
