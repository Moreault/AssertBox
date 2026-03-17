using System.Reflection;

namespace ToolBX.AssertBox;

public readonly record struct Assertions<T>(T Subject, string SubjectExpression)
{
    public Assertions<TException> Throw<TException>() where TException : Exception
    {
        if (Subject is not Delegate d)
            throw new InvalidOperationException("Throw can only be called on delegate assertions.");

        try
        {
            d.DynamicInvoke();
        }
        catch (TargetInvocationException tie) when (tie.InnerException is TException ex)
        {
            return new Assertions<TException>(ex, SubjectExpression);
        }
        catch (TargetInvocationException tie) when (tie.InnerException is not null)
        {
            Fail.With(MessageBuilder.Expected(SubjectExpression, $"to throw {typeof(TException).Name}", tie.InnerException.GetType().Name));
        }
        catch (TException ex)
        {
            return new Assertions<TException>(ex, SubjectExpression);
        }
        catch (Exception ex)
        {
            Fail.With(MessageBuilder.Expected(SubjectExpression, $"to throw {typeof(TException).Name}", ex.GetType().Name));
        }

        Fail.With(MessageBuilder.Expected(SubjectExpression, $"to throw {typeof(TException).Name}", "no exception"));
        return default; // unreachable
    }

    public Assertions<T> NotThrow()
    {
        if (Subject is not Delegate d)
            throw new InvalidOperationException("NotThrow can only be called on delegate assertions.");

        try
        {
            d.DynamicInvoke();
        }
        catch (TargetInvocationException tie) when (tie.InnerException is not null)
        {
            Fail.With(MessageBuilder.Expected(SubjectExpression, "not to throw", tie.InnerException.GetType().Name));
        }
        catch (Exception ex)
        {
            Fail.With(MessageBuilder.Expected(SubjectExpression, "not to throw", ex.GetType().Name));
        }
        return this;
    }

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
