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
        where TCollection : IEnumerable<TElement>
    {
        var subjectList = a.Subject.ToList();
        var expectedList = expected.ToList();

        var comparer = EqualityComparer<TElement>.Default;
        var matched = new bool[expectedList.Count];
        var areEquivalent = subjectList.Count == expectedList.Count;

        if (areEquivalent)
        {
            foreach (var item in subjectList)
            {
                var found = false;
                for (var i = 0; i < expectedList.Count; i++)
                {
                    if (!matched[i] && comparer.Equals(item, expectedList[i]))
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
