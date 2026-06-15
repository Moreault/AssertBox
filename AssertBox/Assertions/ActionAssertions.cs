namespace ToolBX.AssertBox.Assertions;


public static class ActionAssertions
{
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
            var matches = MatchesWildcard(a.Subject.Message, expected);
            Fail.When(
                !matches,
                MessageBuilder.Expected(a.SubjectExpression, $"exception message to match {MessageBuilder.Format(expected)}", a.Subject.Message));
            return a;
        }

        public Assertions<TException> WithInnerException<TInner>() where TInner : Exception
        {
            Fail.When(
                a.Subject.InnerException is not TInner,
                () => MessageBuilder.Expected(a.SubjectExpression, $"to have inner exception of type {typeof(TInner).Name}", a.Subject.InnerException?.GetType().Name ?? "<null>"));
            return a;
        }

        public Assertions<TException> WithInnerExceptionExactly<TInner>() where TInner : Exception
        {
            Fail.When(
                a.Subject.InnerException is null || a.Subject.InnerException.GetType() != typeof(TInner),
                () => MessageBuilder.Expected(a.SubjectExpression, $"to have inner exception of exactly type {typeof(TInner).Name}", a.Subject.InnerException?.GetType().Name ?? "<null>"));
            return a;
        }

        public Assertions<TException> Where(Func<TException, bool> predicate)
        {
            Fail.When(
                !predicate(a.Subject),
                MessageBuilder.Expected(a.SubjectExpression, "exception to satisfy the given predicate", a.Subject.Message));
            return a;
        }
    }

    extension<TException>(Assertions<TException> a) where TException : ArgumentException
    {
        public Assertions<TException> WithParameterName(string expected)
        {
            Fail.When(
                !string.Equals(a.Subject.ParamName, expected, StringComparison.Ordinal),
                () => MessageBuilder.Expected(a.SubjectExpression, $"exception parameter name to be {MessageBuilder.Format(expected)}", a.Subject.ParamName));
            return a;
        }
    }

    private static bool MatchesWildcard(string actual, string pattern)
    {
        if (!pattern.Contains('*'))
            return actual.Contains(pattern, StringComparison.Ordinal);

        var regexPattern = "^" + System.Text.RegularExpressions.Regex.Escape(pattern).Replace("\\*", ".*") + "$";
        return System.Text.RegularExpressions.Regex.IsMatch(actual, regexPattern, System.Text.RegularExpressions.RegexOptions.Singleline);
    }
}
