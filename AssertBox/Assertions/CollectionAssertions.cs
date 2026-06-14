using System.Collections.Immutable;

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

        public Assertions<T> OnlyHaveUniqueItems()
        {
            var seen = new HashSet<object?>();
            var hasDuplicate = false;
            foreach (var item in a.Subject)
            {
                if (!seen.Add(item))
                {
                    hasDuplicate = true;
                    break;
                }
            }

            Fail.When(
                hasDuplicate,
                () => MessageBuilder.Expected(a.SubjectExpression, "to only have unique items", a.Subject));
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
        this Assertions<TCollection> a, IEnumerable<TElement> expected)
        where TCollection : IEnumerable
    {
        var subjectList = ToObjectList(a.Subject);
        var missing = new List<object?>();
        foreach (var item in expected)
        {
            if (!subjectList.Any(s => AreEqual(s, item)))
                missing.Add(item);
        }

        Fail.When(
            missing.Count > 0,
            () => MessageBuilder.Expected(a.SubjectExpression, $"to contain {MessageBuilder.Format(expected)}", a.Subject));
        return a;
    }

    public static Assertions<TCollection> NotContain<TCollection, TElement>(
        this Assertions<TCollection> a, IEnumerable<TElement> unexpected)
        where TCollection : IEnumerable
    {
        var subjectList = ToObjectList(a.Subject);
        var present = new List<object?>();
        foreach (var item in unexpected)
        {
            if (subjectList.Any(s => AreEqual(s, item)))
                present.Add(item);
        }

        Fail.When(
            present.Count > 0,
            () => MessageBuilder.Expected(a.SubjectExpression, $"not to contain {MessageBuilder.Format(unexpected)}", a.Subject));
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

    public static Assertions<TCollection> NotContain<TCollection, TElement>(
        this Assertions<TCollection> a, Func<TElement, bool> predicate)
        where TCollection : IEnumerable<TElement>
        => NotContainMatching(a, predicate);

    public static Assertions<TCollection> AllSatisfy<TCollection, TElement>(
        this Assertions<TCollection> a, Func<TElement, bool> predicate)
        where TCollection : IEnumerable<TElement>
    {
        Fail.When(
            !a.Subject.All(predicate),
            MessageBuilder.Expected(a.SubjectExpression, "all elements to satisfy the predicate", a.Subject));
        return a;
    }

    public static Assertions<TCollection> AllSatisfy<TCollection, TElement>(
        this Assertions<TCollection> a, Action<TElement> inspector)
        where TCollection : IEnumerable<TElement>
    {
        foreach (var item in a.Subject)
            inspector(item);
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
        where TCollection : IEnumerable
    {
        var subjectList = ToObjectList(a.Subject);
        var expectedList = ToObjectList(expected);

        var matched = new bool[expectedList.Count];
        var areEquivalent = subjectList.Count == expectedList.Count;

        if (areEquivalent)
        {
            foreach (var item in subjectList)
            {
                var found = false;
                for (var i = 0; i < expectedList.Count; i++)
                {
                    if (!matched[i] && ElementsEqual(item, expectedList[i]))
                    {
                        matched[i] = true;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    areEquivalent = false;
                    break;
                }
            }
        }

        Fail.When(
            !areEquivalent,
            () => MessageBuilder.Expected(a.SubjectExpression, $"to be equivalent to {MessageBuilder.Format(expected)}", a.Subject));
        return a;
    }

    public static Assertions<TCollection> ContainEquivalentOf<TCollection, TElement>(
        this Assertions<TCollection> a, TElement expected)
        where TCollection : IEnumerable
    {
        var found = false;
        foreach (var item in a.Subject)
        {
            if (DeepEquivalence.AreEquivalent(item, expected, out _))
            {
                found = true;
                break;
            }
        }

        Fail.When(
            !found,
            () => MessageBuilder.Expected(a.SubjectExpression, $"to contain an element equivalent to {MessageBuilder.Format(expected)}", a.Subject));
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

    public static Assertions<TCollection> ContainInOrder<TCollection, TElement>(
        this Assertions<TCollection> a, IEnumerable<TElement> expected)
        where TCollection : IEnumerable<TElement>
    {
        var expectedList = expected as IReadOnlyList<TElement> ?? expected.ToList();
        Fail.When(
            !ContainsInOrder(a.Subject, expectedList),
            () => MessageBuilder.Expected(a.SubjectExpression, $"to contain {MessageBuilder.Format(expectedList)} in order", a.Subject));
        return a;
    }

    public static Assertions<TCollection> NotContainInOrder<TCollection, TElement>(
        this Assertions<TCollection> a, params TElement[] unexpected)
        where TCollection : IEnumerable<TElement>
        => a.NotContainInOrder((IEnumerable<TElement>)unexpected);

    public static Assertions<TCollection> NotContainInOrder<TCollection, TElement>(
        this Assertions<TCollection> a, IEnumerable<TElement> unexpected)
        where TCollection : IEnumerable<TElement>
    {
        var unexpectedList = unexpected as IReadOnlyList<TElement> ?? unexpected.ToList();
        Fail.When(
            unexpectedList.Count > 0 && ContainsInOrder(a.Subject, unexpectedList),
            () => MessageBuilder.Expected(a.SubjectExpression, $"not to contain {MessageBuilder.Format(unexpectedList)} in order", a.Subject));
        return a;
    }

    private static bool ContainsInOrder<TElement>(IEnumerable<TElement> subject, IReadOnlyList<TElement> expected)
    {
        var idx = 0;
        foreach (var item in subject)
        {
            if (idx < expected.Count && EqualityComparer<TElement>.Default.Equals(item, expected[idx]))
                idx++;
        }
        return idx == expected.Count;
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

    // Concrete overloads for predicate methods to enable lambda type inference.
    // The generic two-type-parameter versions above require explicit Func<T, bool> casts
    // because the compiler can't infer TElement from a lambda. These single-type-parameter
    // overloads resolve TElement from the receiver type, enabling natural lambda syntax.

    extension<TElement>(Assertions<TElement[]> a)
    {
        public Assertions<TElement[]> Contain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.Any(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
            return a;
        }

        public Assertions<TElement[]> AllSatisfy(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "all elements to satisfy the predicate", a.Subject));
            return a;
        }

        public Assertions<TElement[]> OnlyContain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to only contain elements matching the predicate", a.Subject));
            return a;
        }

        public Assertions<TElement[]> AllSatisfy(Action<TElement> inspector)
        {
            foreach (var item in a.Subject)
                inspector(item);
            return a;
        }
    }

    extension<TElement>(Assertions<List<TElement>> a)
    {
        public Assertions<List<TElement>> Contain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.Any(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
            return a;
        }

        public Assertions<List<TElement>> AllSatisfy(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "all elements to satisfy the predicate", a.Subject));
            return a;
        }

        public Assertions<List<TElement>> OnlyContain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to only contain elements matching the predicate", a.Subject));
            return a;
        }

        public Assertions<List<TElement>> AllSatisfy(Action<TElement> inspector)
        {
            foreach (var item in a.Subject)
                inspector(item);
            return a;
        }
    }

    extension<TElement>(Assertions<IEnumerable<TElement>> a)
    {
        public Assertions<IEnumerable<TElement>> Contain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.Any(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
            return a;
        }

        public Assertions<IEnumerable<TElement>> AllSatisfy(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "all elements to satisfy the predicate", a.Subject));
            return a;
        }

        public Assertions<IEnumerable<TElement>> OnlyContain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to only contain elements matching the predicate", a.Subject));
            return a;
        }

        public Assertions<IEnumerable<TElement>> AllSatisfy(Action<TElement> inspector)
        {
            foreach (var item in a.Subject)
                inspector(item);
            return a;
        }
    }

    extension<TElement>(Assertions<IList<TElement>> a)
    {
        public Assertions<IList<TElement>> Contain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.Any(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
            return a;
        }

        public Assertions<IList<TElement>> AllSatisfy(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "all elements to satisfy the predicate", a.Subject));
            return a;
        }

        public Assertions<IList<TElement>> OnlyContain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to only contain elements matching the predicate", a.Subject));
            return a;
        }

        public Assertions<IList<TElement>> AllSatisfy(Action<TElement> inspector)
        {
            foreach (var item in a.Subject)
                inspector(item);
            return a;
        }
    }

    extension<TElement>(Assertions<ICollection<TElement>> a)
    {
        public Assertions<ICollection<TElement>> Contain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.Any(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
            return a;
        }

        public Assertions<ICollection<TElement>> AllSatisfy(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "all elements to satisfy the predicate", a.Subject));
            return a;
        }

        public Assertions<ICollection<TElement>> OnlyContain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to only contain elements matching the predicate", a.Subject));
            return a;
        }

        public Assertions<ICollection<TElement>> AllSatisfy(Action<TElement> inspector)
        {
            foreach (var item in a.Subject)
                inspector(item);
            return a;
        }
    }

    extension<TElement>(Assertions<IReadOnlyList<TElement>> a)
    {
        public Assertions<IReadOnlyList<TElement>> Contain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.Any(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
            return a;
        }

        public Assertions<IReadOnlyList<TElement>> AllSatisfy(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "all elements to satisfy the predicate", a.Subject));
            return a;
        }

        public Assertions<IReadOnlyList<TElement>> OnlyContain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to only contain elements matching the predicate", a.Subject));
            return a;
        }

        public Assertions<IReadOnlyList<TElement>> AllSatisfy(Action<TElement> inspector)
        {
            foreach (var item in a.Subject)
                inspector(item);
            return a;
        }
    }

    extension<TElement>(Assertions<IReadOnlyCollection<TElement>> a)
    {
        public Assertions<IReadOnlyCollection<TElement>> Contain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.Any(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
            return a;
        }

        public Assertions<IReadOnlyCollection<TElement>> AllSatisfy(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "all elements to satisfy the predicate", a.Subject));
            return a;
        }

        public Assertions<IReadOnlyCollection<TElement>> OnlyContain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to only contain elements matching the predicate", a.Subject));
            return a;
        }

        public Assertions<IReadOnlyCollection<TElement>> AllSatisfy(Action<TElement> inspector)
        {
            foreach (var item in a.Subject)
                inspector(item);
            return a;
        }
    }

    extension<TElement>(Assertions<HashSet<TElement>> a)
    {
        public Assertions<HashSet<TElement>> Contain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.Any(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
            return a;
        }

        public Assertions<HashSet<TElement>> AllSatisfy(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "all elements to satisfy the predicate", a.Subject));
            return a;
        }

        public Assertions<HashSet<TElement>> OnlyContain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to only contain elements matching the predicate", a.Subject));
            return a;
        }

        public Assertions<HashSet<TElement>> AllSatisfy(Action<TElement> inspector)
        {
            foreach (var item in a.Subject)
                inspector(item);
            return a;
        }
    }

    extension<TElement>(Assertions<ImmutableList<TElement>> a)
    {
        public Assertions<ImmutableList<TElement>> Contain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.Any(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
            return a;
        }

        public Assertions<ImmutableList<TElement>> AllSatisfy(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "all elements to satisfy the predicate", a.Subject));
            return a;
        }

        public Assertions<ImmutableList<TElement>> OnlyContain(Func<TElement, bool> predicate)
        {
            Fail.When(
                !a.Subject.All(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to only contain elements matching the predicate", a.Subject));
            return a;
        }

        public Assertions<ImmutableList<TElement>> AllSatisfy(Action<TElement> inspector)
        {
            foreach (var item in a.Subject)
                inspector(item);
            return a;
        }
    }

    extension<TElement>(Assertions<TElement[]> a)
    {
        public Assertions<TElement[]> NotContain(Func<TElement, bool> predicate)
            => NotContainMatching(a, predicate);
    }

    extension<TElement>(Assertions<List<TElement>> a)
    {
        public Assertions<List<TElement>> NotContain(Func<TElement, bool> predicate)
            => NotContainMatching(a, predicate);
    }

    extension<TElement>(Assertions<IEnumerable<TElement>> a)
    {
        public Assertions<IEnumerable<TElement>> NotContain(Func<TElement, bool> predicate)
            => NotContainMatching(a, predicate);
    }

    extension<TElement>(Assertions<IList<TElement>> a)
    {
        public Assertions<IList<TElement>> NotContain(Func<TElement, bool> predicate)
            => NotContainMatching(a, predicate);
    }

    extension<TElement>(Assertions<ICollection<TElement>> a)
    {
        public Assertions<ICollection<TElement>> NotContain(Func<TElement, bool> predicate)
            => NotContainMatching(a, predicate);
    }

    extension<TElement>(Assertions<IReadOnlyList<TElement>> a)
    {
        public Assertions<IReadOnlyList<TElement>> NotContain(Func<TElement, bool> predicate)
            => NotContainMatching(a, predicate);
    }

    extension<TElement>(Assertions<IReadOnlyCollection<TElement>> a)
    {
        public Assertions<IReadOnlyCollection<TElement>> NotContain(Func<TElement, bool> predicate)
            => NotContainMatching(a, predicate);
    }

    extension<TElement>(Assertions<HashSet<TElement>> a)
    {
        public Assertions<HashSet<TElement>> NotContain(Func<TElement, bool> predicate)
            => NotContainMatching(a, predicate);
    }

    extension<TElement>(Assertions<ImmutableList<TElement>> a)
    {
        public Assertions<ImmutableList<TElement>> NotContain(Func<TElement, bool> predicate)
            => NotContainMatching(a, predicate);
    }

    extension<TKey, TValue>(Assertions<Dictionary<TKey, TValue>> a) where TKey : notnull
    {
        public Assertions<Dictionary<TKey, TValue>> Contain(Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            Fail.When(
                !a.Subject.Any(predicate),
                MessageBuilder.Expected(a.SubjectExpression, "to contain an element matching the predicate", a.Subject));
            return a;
        }

        public Assertions<Dictionary<TKey, TValue>> NotContain(Func<KeyValuePair<TKey, TValue>, bool> predicate)
            => NotContainMatching(a, predicate);
    }

    private static Assertions<TCollection> NotContainMatching<TCollection, TElement>(
        Assertions<TCollection> a, Func<TElement, bool> predicate)
        where TCollection : IEnumerable<TElement>
    {
        Fail.When(
            a.Subject.Any(predicate),
            MessageBuilder.Expected(a.SubjectExpression, "not to contain an element matching the predicate", a.Subject));
        return a;
    }

    private static List<object?> ToObjectList(IEnumerable source)
    {
        var list = new List<object?>();
        foreach (var item in source)
            list.Add(item);
        return list;
    }

    private static bool AreEqual(object? subject, object? expected)
    {
        if (ReferenceEquals(subject, expected))
            return true;
        if (subject is null || expected is null)
            return false;
        return subject.Equals(expected) || expected.Equals(subject);
    }

    private static bool ElementsEqual(object? subject, object? expected) =>
        AreEqual(subject, expected) || DeepEquivalence.AreEquivalent(subject, expected, out _);

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
