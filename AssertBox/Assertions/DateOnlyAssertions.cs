namespace ToolBX.AssertBox.Assertions;

public static class DateOnlyAssertions
{
    extension(Assertions<DateOnly> a)
    {
        public Assertions<DateOnly> BeBefore(DateOnly expected)
        {
            Fail.When(
                a.Subject >= expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be before {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<DateOnly> BeAfter(DateOnly expected)
        {
            Fail.When(
                a.Subject <= expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be after {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<DateOnly> BeOnOrBefore(DateOnly expected)
        {
            Fail.When(
                a.Subject > expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be on or before {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<DateOnly> BeOnOrAfter(DateOnly expected)
        {
            Fail.When(
                a.Subject < expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to be on or after {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<DateOnly> BeCloseTo(DateOnly expected, int precisionInDays)
        {
            var diff = Math.Abs(a.Subject.DayNumber - expected.DayNumber);
            Fail.When(
                diff > precisionInDays,
                MessageBuilder.Expected(a.SubjectExpression, $"to be within {precisionInDays} day(s) of {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<DateOnly> NotBeCloseTo(DateOnly expected, int precisionInDays)
        {
            var diff = Math.Abs(a.Subject.DayNumber - expected.DayNumber);
            Fail.When(
                diff <= precisionInDays,
                MessageBuilder.Expected(a.SubjectExpression, $"not to be within {precisionInDays} day(s) of {expected:O}", a.Subject.ToString("O")));
            return a;
        }

        public Assertions<DateOnly> HaveYear(int expected)
        {
            Fail.When(
                a.Subject.Year != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have year {expected}", a.Subject.Year));
            return a;
        }

        public Assertions<DateOnly> HaveMonth(int expected)
        {
            Fail.When(
                a.Subject.Month != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have month {expected}", a.Subject.Month));
            return a;
        }

        public Assertions<DateOnly> HaveDay(int expected)
        {
            Fail.When(
                a.Subject.Day != expected,
                MessageBuilder.Expected(a.SubjectExpression, $"to have day {expected}", a.Subject.Day));
            return a;
        }
    }
}
