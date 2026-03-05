namespace AssertBox.Assertions;

public static class NumericAssertions
{
    extension<T>(Assertions<T> a) where T : INumber<T>
    {
        public Assertions<T> BePositive()
        {
            Fail.When(
                a.Subject <= T.Zero,
                MessageBuilder.Expected(a.SubjectExpression, "to be positive", a.Subject));
            return a;
        }

        public Assertions<T> BeNegative()
        {
            Fail.When(
                a.Subject >= T.Zero,
                MessageBuilder.Expected(a.SubjectExpression, "to be negative", a.Subject));
            return a;
        }

        public Assertions<T> BeZero()
        {
            Fail.When(
                a.Subject != T.Zero,
                MessageBuilder.Expected(a.SubjectExpression, "to be zero", a.Subject));
            return a;
        }

        public Assertions<T> BeApproximately(T expected, T precision)
        {
            var diff = a.Subject > expected ? a.Subject - expected : expected - a.Subject;
            Fail.When(
                diff > precision,
                MessageBuilder.Expected(a.SubjectExpression, $"to be approximately {MessageBuilder.Format(expected)} +/- {MessageBuilder.Format(precision)}", a.Subject));
            return a;
        }
    }
}
