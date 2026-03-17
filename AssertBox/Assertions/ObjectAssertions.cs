namespace ToolBX.AssertBox.Assertions;

public static class ObjectAssertions
{
    extension<T>(Assertions<T> a)
    {
        public Assertions<T> Be(T expected)
        {
            Fail.When(
                !Equals(a.Subject, expected),
                MessageBuilder.Expected(a.SubjectExpression, $"to be {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<T> NotBe(T expected)
        {
            Fail.When(
                Equals(a.Subject, expected),
                MessageBuilder.Expected(a.SubjectExpression, $"not to be {MessageBuilder.Format(expected)}", MessageBuilder.OmitActual));
            return a;
        }

        public Assertions<T> BeNull()
        {
            Fail.When(
                a.Subject is not null,
                MessageBuilder.Expected(a.SubjectExpression, "to be <null>", a.Subject));
            return a;
        }

        public Assertions<T> NotBeNull()
        {
            Fail.When(
                a.Subject is null,
                MessageBuilder.Expected(a.SubjectExpression, "not to be <null>", MessageBuilder.OmitActual));
            return a;
        }

        public Assertions<T> Satisfy(Func<T, bool> predicate)
        {
            Fail.When(
                !predicate(a.Subject),
                MessageBuilder.Expected(a.SubjectExpression, "to satisfy the given predicate", a.Subject));
            return a;
        }

        public Assertions<T> BeOneOf(params IEnumerable<T> expected)
        {
            Fail.When(
                !expected.Contains(a.Subject),
                MessageBuilder.Expected(a.SubjectExpression, $"to be one of {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }
    }

    extension(Assertions<uint> a)
    {
        public Assertions<uint> Be(uint expected)
        {
            Fail.When(
                a.Subject != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<uint> NotBe(uint expected)
        {
            Fail.When(
                a.Subject == expected,
                MessageBuilder.Expected(a.SubjectExpression, $"not to be {MessageBuilder.Format(expected)}", MessageBuilder.OmitActual));
            return a;
        }
    }

    extension(Assertions<ulong> a)
    {
        public Assertions<ulong> Be(ulong expected)
        {
            Fail.When(
                a.Subject != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<ulong> NotBe(ulong expected)
        {
            Fail.When(
                a.Subject == expected,
                MessageBuilder.Expected(a.SubjectExpression, $"not to be {MessageBuilder.Format(expected)}", MessageBuilder.OmitActual));
            return a;
        }
    }

    extension<T>(Assertions<T> a) where T : class?
    {
        public Assertions<T> BeSameAs(T expected)
        {
            Fail.When(
                !ReferenceEquals(a.Subject, expected),
                MessageBuilder.Expected(a.SubjectExpression, "to be the same reference", MessageBuilder.OmitActual));
            return a;
        }

        public Assertions<T> NotBeSameAs(T expected)
        {
            Fail.When(
                ReferenceEquals(a.Subject, expected),
                MessageBuilder.Expected(a.SubjectExpression, "not to be the same reference", MessageBuilder.OmitActual));
            return a;
        }
    }

    public static Assertions<T> BeEquivalentTo<T>(this Assertions<T> a, T expected)
    {
        DeepEquivalence.AreEquivalent(a.Subject, expected, out var difference);
        Fail.When(
            difference is not null,
            () => MessageBuilder.Expected(a.SubjectExpression, $"to be equivalent to {MessageBuilder.Format(expected)} but found a difference at '{difference}'", MessageBuilder.OmitActual));
        return a;
    }

    public static Assertions<T> NotBeEquivalentTo<T>(this Assertions<T> a, T expected)
    {
        DeepEquivalence.AreEquivalent(a.Subject, expected, out var difference);
        Fail.When(
            difference is null,
            MessageBuilder.Expected(a.SubjectExpression, "not to be equivalent to the given value", MessageBuilder.OmitActual));
        return a;
    }

    private static bool Equals<T>(T? left, T? right) =>
        EqualityComparer<T>.Default.Equals(left!, right!);
}
