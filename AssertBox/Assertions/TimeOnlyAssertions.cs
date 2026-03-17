namespace ToolBX.AssertBox.Assertions;

public static class TimeOnlyAssertions
{
    extension(Assertions<TimeOnly> a)
    {
        public Assertions<TimeOnly> BeBefore(TimeOnly expected)
        {
            Fail.When(
                a.Subject >= expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be before {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<TimeOnly> BeAfter(TimeOnly expected)
        {
            Fail.When(
                a.Subject <= expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be after {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<TimeOnly> BeOnOrBefore(TimeOnly expected)
        {
            Fail.When(
                a.Subject > expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be on or before {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<TimeOnly> BeOnOrAfter(TimeOnly expected)
        {
            Fail.When(
                a.Subject < expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be on or after {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<TimeOnly> BeCloseTo(TimeOnly expected, TimeSpan precision)
        {
            var diff = AbsDifference(a.Subject, expected);
            Fail.When(
                diff > precision,
                MessageBuilder.Expected(a.SubjectExpression, $"to be within {precision} of {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<TimeOnly> NotBeCloseTo(TimeOnly expected, TimeSpan precision)
        {
            var diff = AbsDifference(a.Subject, expected);
            Fail.When(
                diff <= precision,
                MessageBuilder.Expected(a.SubjectExpression, $"not to be within {precision} of {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<TimeOnly> HaveHour(int expected)
        {
            Fail.When(
                a.Subject.Hour != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have hour {expected}", a.Subject.Hour));
            return a;
        }

        public Assertions<TimeOnly> HaveMinute(int expected)
        {
            Fail.When(
                a.Subject.Minute != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have minute {expected}", a.Subject.Minute));
            return a;
        }

        public Assertions<TimeOnly> HaveSecond(int expected)
        {
            Fail.When(
                a.Subject.Second != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have second {expected}", a.Subject.Second));
            return a;
        }

        public Assertions<TimeOnly> HaveMillisecond(int expected)
        {
            Fail.When(
                a.Subject.Millisecond != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have millisecond {expected}", a.Subject.Millisecond));
            return a;
        }
    }

    private static TimeSpan AbsDifference(TimeOnly a, TimeOnly b)
    {
        var diff = a - b;
        return diff < TimeSpan.Zero ? diff.Negate() : diff;
    }
}
