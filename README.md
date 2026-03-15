![AssertBox](https://github.com/Moreault/AssertBox/blob/master/assertbox.png)

# What is it?
A lightweight, straightforward and fluent assertion library for .NET. It is meant to be used within a unit test project.

# How do I use it?

```cs
[TestMethod]
public void Test_Bools()
{
    var result = Instance.IsTrue();

    //Will throw an assertion excetion if the result is different from 'true'
    result.Should().BeTrue();
    //Same for 'false
    result.Should().BeFalse();
}
```

```cs
[TestMethod]
public void Test_Numbers()
{
    var result = Instance.GetNumber();

    //Asserts that the returned number is 12
    result.Should().Be(12);
    //Asserts that it's positive
    result.Should().BePositive();
    //Asserts that it's negative
    result.Should().BeNegative();
    //Asserts that it's zero
    result.Should().BeZero();
    //For floating point numbers
    result.Should().BeApproximately(3.15);
    //Asserts that it's greater than 10
    result.Should().BeGreaterThan(10);
    //Assertions can also be chained
    result.Should().BeGreaterThan(0).BeLessThan(100).BeInRange(1, 99);
}
```

```cs
[TestMethod]
public void Test_Strings()
{
    var result = Instance.GetName();

    result.Should().Be("Roger");
    result.Should().Contain("og");
    result.Should().StartWith("Ro");
    result.Should().EndWith("er");
    result.Should().Match(@"\w+");
    result.Should().HaveLength(5);
    result.Should().NotBeEmpty();
    result.Should().NotBeNullOrEmpty();
    //Case-insensitive comparison
    result.Should().BeEquivalentTo("ROGER");
}
```

```cs
[TestMethod]
public void Test_Objects()
{
    var result = Instance.GetPerson();

    result.Should().NotBeNull();
    result.Should().BeOfType<Person>();
    result.Should().BeAssignableTo<IEntity>();
    result.Should().Satisfy(x => x.Age > 18);
    result.Should().BeSameAs(result);
    result.Should().NotBeSameAs(new Person());
    //Deep structural comparison of all public properties
    result.Should().BeEquivalentTo(new Person { Name = "Roger", Age = 35 });
}
```

```cs
[TestMethod]
public void Test_DateTimes()
{
    var result = Instance.GetDate();

    result.Should().BeBefore(DateTime.Now);
    result.Should().BeAfter(new DateTime(2020, 1, 1));
    result.Should().BeOnOrBefore(DateTime.Now);
    result.Should().BeOnOrAfter(new DateTime(2020, 1, 1));
    result.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    result.Should().HaveYear(2025).HaveMonth(3).HaveDay(15);
}
```

```cs
[TestMethod]
public void Test_Collections()
{
    var result = Instance.GetNames();

    result.Should().NotBeEmpty();
    result.Should().HaveCount(3);
    result.Should().HaveCountGreaterThan(1);
    result.Should().HaveCountLessThan(10);
    result.Should().Contain("Roger");
    result.Should().NotContain("Seb");
    result.Should().Contain(x => x.StartsWith("R"));
    result.Should().AllSatisfy(x => x.Length > 0);
    result.Should().BeEquivalentTo(new[] { "Terry", "Roger", "Bob" });
    result.Should().ContainInOrder("Roger", "Bob");
    result.Should().BeInAscendingOrder();
}
```

```cs
[TestMethod]
public void Test_Exceptions()
{
    Action action = () => Instance.DoSomethingDangerous();

    action.Should().Throw<InvalidOperationException>()
        .WithMessage("can't do that")
        .WithInnerException<ArgumentException>();

    Action safe = () => Instance.DoSomethingSafe();
    safe.Should().NotThrow();
}
```

```cs
[TestMethod]
public async Task Test_AsyncExceptions()
{
    Func<Task> action = () => Instance.DoSomethingDangerousAsync();

    (await action.Should().ThrowAsync<InvalidOperationException>())
        .WithMessage("can't do that");

    Func<Task> safe = () => Instance.DoSomethingSafeAsync();
    await safe.Should().NotThrowAsync();
}
```

```cs
[TestMethod]
public void Test_ArgumentExceptions()
{
    Action action = () => Instance.DoSomething(null!);

    action.Should().Throw<ArgumentNullException>()
        .WithParameterName("name");
}
```