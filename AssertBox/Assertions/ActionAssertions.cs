namespace ToolBX.AssertBox.Assertions;

public static class ActionAssertions
{
    extension(Assertions<Action> a)
    {
        public Assertions<TException> Throw<TException>() where TException : Exception
        {
            try
            {
                a.Subject();
            }
            catch (TException ex)
            {
                return new Assertions<TException>(ex, a.SubjectExpression);
            }
            catch (Exception ex)
            {
                Fail.With(MessageBuilder.Expected(a.SubjectExpression, $"to throw {typeof(TException).Name}", ex.GetType().Name));
            }

            Fail.With(MessageBuilder.Expected(a.SubjectExpression, $"to throw {typeof(TException).Name}", "no exception"));
            return default; // unreachable
        }

        public Assertions<Action> NotThrow()
        {
            try
            {
                a.Subject();
            }
            catch (Exception ex)
            {
                Fail.With(MessageBuilder.Expected(a.SubjectExpression, "not to throw", ex.GetType().Name));
            }
            return a;
        }
    }

    extension(Assertions<Func<Task>> a)
    {
        public async Task<Assertions<TException>> ThrowAsync<TException>() where TException : Exception
        {
            try
            {
                await a.Subject();
            }
            catch (TException ex)
            {
                return new Assertions<TException>(ex, a.SubjectExpression);
            }
            catch (Exception ex)
            {
                Fail.With(MessageBuilder.Expected(a.SubjectExpression, $"to throw {typeof(TException).Name}", ex.GetType().Name));
            }

            Fail.With(MessageBuilder.Expected(a.SubjectExpression, $"to throw {typeof(TException).Name}", "no exception"));
            return default; // unreachable
        }

        public async Task<Assertions<Func<Task>>> NotThrowAsync()
        {
            try
            {
                await a.Subject();
            }
            catch (Exception ex)
            {
                Fail.With(MessageBuilder.Expected(a.SubjectExpression, "not to throw", ex.GetType().Name));
            }
            return a;
        }
    }

    extension<TException>(Assertions<TException> a) where TException : Exception
    {
        public Assertions<TException> WithMessage(string expected)
        {
            Fail.When(
                !a.Subject.Message.Contains(expected, StringComparison.Ordinal),
                MessageBuilder.Expected(a.SubjectExpression, $"exception message to contain {MessageBuilder.Format(expected)}", a.Subject.Message));
            return a;
        }

        public Assertions<TException> WithInnerException<TInner>() where TInner : Exception
        {
            Fail.When(
                a.Subject.InnerException is not TInner,
                () => MessageBuilder.Expected(a.SubjectExpression, $"to have inner exception of type {typeof(TInner).Name}", a.Subject.InnerException?.GetType().Name ?? "<null>"));
            return a;
        }
    }
}
