namespace AssertBox.Assertions;

public static class ComparableAssertions
{
    extension<T>(Assertions<T> a) where T : IComparable<T>
    {
        public Assertions<T> BeGreaterThan(T expected)
        {
            Fail.When(
                a.Subject.CompareTo(expected) <= 0,
                MessageBuilder.Expected(a.SubjectExpression, $"to be greater than {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<T> BeGreaterThanOrEqualTo(T expected)
        {
            Fail.When(
                a.Subject.CompareTo(expected) < 0,
                MessageBuilder.Expected(a.SubjectExpression, $"to be greater than or equal to {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<T> BeLessThan(T expected)
        {
            Fail.When(
                a.Subject.CompareTo(expected) >= 0,
                MessageBuilder.Expected(a.SubjectExpression, $"to be less than {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<T> BeLessThanOrEqualTo(T expected)
        {
            Fail.When(
                a.Subject.CompareTo(expected) > 0,
                MessageBuilder.Expected(a.SubjectExpression, $"to be less than or equal to {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<T> BeInRange(T min, T max)
        {
            Fail.When(
                a.Subject.CompareTo(min) < 0 || a.Subject.CompareTo(max) > 0,
                MessageBuilder.Expected(a.SubjectExpression, $"to be in range [{MessageBuilder.Format(min)}, {MessageBuilder.Format(max)}]", a.Subject));
            return a;
        }
    }
}
