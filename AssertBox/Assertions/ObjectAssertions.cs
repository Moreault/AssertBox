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

    public static Assertions<T> BeOfType<TExpected, T>(this Assertions<T> a)
    {
        Fail.When(
            a.Subject is null || a.Subject.GetType() != typeof(TExpected),
            () => MessageBuilder.Expected(a.SubjectExpression, $"to be of type {typeof(TExpected).Name}", a.Subject?.GetType().Name ?? "<null>"));
        return a;
    }

    public static Assertions<T> BeAssignableTo<TExpected, T>(this Assertions<T> a)
    {
        Fail.When(
            a.Subject is not TExpected,
            () => MessageBuilder.Expected(a.SubjectExpression, $"to be assignable to {typeof(TExpected).Name}", a.Subject?.GetType().Name ?? "<null>"));
        return a;
    }

    public static Assertions<T> Satisfy<T>(this Assertions<T> a, Func<T, bool> predicate)
    {
        Fail.When(
            !predicate(a.Subject),
            MessageBuilder.Expected(a.SubjectExpression, "to satisfy the given predicate", a.Subject));
        return a;
    }

    private static bool Equals<T>(T? left, T? right) =>
        EqualityComparer<T>.Default.Equals(left!, right!);
}
