namespace ToolBX.AssertBox.Assertions;

public static class TimeSpanAssertions
{
    extension(Assertions<TimeSpan> a)
    {
        public Assertions<TimeSpan> BeCloseTo(TimeSpan expected, TimeSpan precision)
        {
            var diff = (a.Subject - expected).Duration();
            Fail.When(
                diff > precision,
                MessageBuilder.Expected(a.SubjectExpression, $"to be within {precision} of {expected}", a.Subject));
            return a;
        }

        public Assertions<TimeSpan> NotBeCloseTo(TimeSpan expected, TimeSpan precision)
        {
            var diff = (a.Subject - expected).Duration();
            Fail.When(
                diff <= precision,
                MessageBuilder.Expected(a.SubjectExpression, $"not to be within {precision} of {expected}", a.Subject));
            return a;
        }
    }
}
