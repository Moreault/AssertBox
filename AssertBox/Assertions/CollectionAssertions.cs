namespace ToolBX.AssertBox.Assertions;

public static class CollectionAssertions
{
    extension<T>(Assertions<T> a) where T : IEnumerable
    {
        public Assertions<T> BeEmpty()
        {
            Fail.When(
                HasAny(a.Subject),
                () => MessageBuilder.Expected(a.SubjectExpression, "to be empty", a.Subject));
            return a;
        }

        public Assertions<T> NotBeEmpty()
        {
            Fail.When(
                !HasAny(a.Subject),
                MessageBuilder.Expected(a.SubjectExpression, "not to be empty", MessageBuilder.OmitActual));
            return a;
        }

        public Assertions<T> HaveCount(int expected)
        {
            var actual = Count(a.Subject);
            Fail.When(
                actual != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have count {expected}", actual));
            return a;
        }
    }

    public static Assertions<TCollection> Contain<TCollection, TElement>(
        this Assertions<TCollection> a, TElement expected)
        where TCollection : IEnumerable<TElement>
    {
        Fail.When(
            !a.Subject.Contains(expected),
            MessageBuilder.Expected(a.SubjectExpression, $"to contain {MessageBuilder.Format(expected)}", a.Subject));
        return a;
    }

    public static Assertions<TCollection> NotContain<TCollection, TElement>(
        this Assertions<TCollection> a, TElement expected)
        where TCollection : IEnumerable<TElement>
    {
        Fail.When(
            a.Subject.Contains(expected),
            MessageBuilder.Expected(a.SubjectExpression, $"not to contain {MessageBuilder.Format(expected)}", a.Subject));
        return a;
    }

    public static Assertions<TCollection> Contain<TCollection, TElement>(
        this Assertions<TCollection> a, Func<TElement, bool> predicate)
        where TCollection : IEnumerable<TElement>
    {
        Fail.When(
            !a.Subject.Any(predicate),
            MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
        return a;
    }

    public static Assertions<TCollection> AllSatisfy<TCollection, TElement>(
        this Assertions<TCollection> a, Func<TElement, bool> predicate)
        where TCollection : IEnumerable<TElement>
    {
        Fail.When(
            !a.Subject.All(predicate),
            MessageBuilder.Expected(a.SubjectExpression, "all elements to satisfy the predicate", a.Subject));
        return a;
    }

    public static Assertions<TCollection> OnlyContain<TCollection, TElement>(
        this Assertions<TCollection> a, Func<TElement, bool> predicate)
        where TCollection : IEnumerable<TElement>
    {
        Fail.When(
            !a.Subject.All(predicate),
            MessageBuilder.Expected(a.SubjectExpression, "to only contain elements matching the predicate", a.Subject));
        return a;
    }

    public static Assertions<TCollection> BeEquivalentTo<TCollection, TElement>(
        this Assertions<TCollection> a, IEnumerable<TElement> expected)
        where TCollection : IEnumerable<TElement>
    {
        var subjectList = a.Subject.OrderBy(x => x).ToList();
        var expectedList = expected.OrderBy(x => x).ToList();
        Fail.When(
            !subjectList.SequenceEqual(expectedList),
            () => MessageBuilder.Expected(a.SubjectExpression, $"to be equivalent to {MessageBuilder.Format(expected)}", a.Subject));
        return a;
    }

    public static Assertions<TCollection> ContainInOrder<TCollection, TElement>(
        this Assertions<TCollection> a, params TElement[] expected)
        where TCollection : IEnumerable<TElement>
    {
        var idx = 0;
        foreach (var item in a.Subject)
        {
            if (idx < expected.Length && EqualityComparer<TElement>.Default.Equals(item, expected[idx]))
                idx++;
        }
        Fail.When(
            idx != expected.Length,
            () => MessageBuilder.Expected(a.SubjectExpression, $"to contain {MessageBuilder.Format(expected)} in order", a.Subject));
        return a;
    }

    public static Assertions<TCollection> BeInAscendingOrder<TCollection, TElement>(this Assertions<TCollection> a)
        where TCollection : IEnumerable<TElement>
        where TElement : IComparable<TElement>
    {
        var list = a.Subject.ToList();
        for (var i = 1; i < list.Count; i++)
        {
            if (list[i].CompareTo(list[i - 1]) < 0)
            {
                Fail.With(MessageBuilder.Expected(a.SubjectExpression, "to be in ascending order", a.Subject));
            }
        }
        return a;
    }

    public static Assertions<TCollection> BeInDescendingOrder<TCollection, TElement>(this Assertions<TCollection> a)
        where TCollection : IEnumerable<TElement>
        where TElement : IComparable<TElement>
    {
        var list = a.Subject.ToList();
        for (var i = 1; i < list.Count; i++)
        {
            if (list[i].CompareTo(list[i - 1]) > 0)
            {
                Fail.With(MessageBuilder.Expected(a.SubjectExpression, "to be in descending order", a.Subject));
            }
        }
        return a;
    }

    extension<T>(Assertions<T> a) where T : IEnumerable
    {
        public Assertions<T> HaveCountGreaterThan(int expected)
        {
            var actual = Count(a.Subject);
            Fail.When(
                actual <= expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have count greater than {expected}", actual));
            return a;
        }

        public Assertions<T> HaveCountLessThan(int expected)
        {
            var actual = Count(a.Subject);
            Fail.When(
                actual >= expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have count less than {expected}", actual));
            return a;
        }
    }

    private static bool HasAny(IEnumerable source)
    {
        var enumerator = source.GetEnumerator();
        try { return enumerator.MoveNext(); }
        finally { (enumerator as IDisposable)?.Dispose(); }
    }

    private static int Count(IEnumerable source)
    {
        var count = 0;
        var enumerator = source.GetEnumerator();
        try { while (enumerator.MoveNext()) count++; }
        finally { (enumerator as IDisposable)?.Dispose(); }
        return count;
    }
}
