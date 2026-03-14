namespace ToolBX.AssertBox;

public readonly record struct Assertions<T>(T Subject, string SubjectExpression)
{
    public Assertions<T> BeOfType<TExpected>()
    {
        var subject = Subject;
        var expression = SubjectExpression;
        Fail.When(
            subject is null || subject.GetType() != typeof(TExpected),
            () => MessageBuilder.Expected(expression, $"to be of type {typeof(TExpected).Name}", subject?.GetType().Name ?? "<null>"));
        return this;
    }

    public Assertions<T> BeAssignableTo<TExpected>()
    {
        var subject = Subject;
        var expression = SubjectExpression;
        Fail.When(
            subject is not TExpected,
            () => MessageBuilder.Expected(expression, $"to be assignable to {typeof(TExpected).Name}", subject?.GetType().Name ?? "<null>"));
        return this;
    }
}
