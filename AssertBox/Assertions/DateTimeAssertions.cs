namespace ToolBX.AssertBox.Assertions;

public static class DateTimeAssertions
{
    extension(Assertions<DateTime> a)
    {
        public Assertions<DateTime> BeBefore(DateTime expected)
        {
            Fail.When(
                a.Subject >= expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be before {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<DateTime> BeAfter(DateTime expected)
        {
            Fail.When(
                a.Subject <= expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be after {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<DateTime> BeOnOrBefore(DateTime expected)
        {
            Fail.When(
                a.Subject > expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be on or before {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<DateTime> BeOnOrAfter(DateTime expected)
        {
            Fail.When(
                a.Subject < expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be on or after {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<DateTime> BeCloseTo(DateTime expected, TimeSpan precision)
        {
            var diff = (a.Subject - expected).Duration();
            Fail.When(
                diff > precision,
                MessageBuilder.Expected(a.SubjectExpression, $"to be within {precision} of {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<DateTime> HaveYear(int expected)
        {
            Fail.When(
                a.Subject.Year != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have year {expected}", a.Subject.Year));
            return a;
        }

        public Assertions<DateTime> HaveMonth(int expected)
        {
            Fail.When(
                a.Subject.Month != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have month {expected}", a.Subject.Month));
            return a;
        }

        public Assertions<DateTime> HaveDay(int expected)
        {
            Fail.When(
                a.Subject.Day != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have day {expected}", a.Subject.Day));
            return a;
        }

        public Assertions<DateTime> HaveHour(int expected)
        {
            Fail.When(
                a.Subject.Hour != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have hour {expected}", a.Subject.Hour));
            return a;
        }

        public Assertions<DateTime> HaveMinute(int expected)
        {
            Fail.When(
                a.Subject.Minute != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have minute {expected}", a.Subject.Minute));
            return a;
        }

        public Assertions<DateTime> HaveSecond(int expected)
        {
            Fail.When(
                a.Subject.Second != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have second {expected}", a.Subject.Second));
            return a;
        }
    }
}
