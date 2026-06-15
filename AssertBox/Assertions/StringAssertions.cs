namespace ToolBX.AssertBox.Assertions;

public static class StringAssertions
{
    extension(Assertions<string> a)
    {
        public Assertions<string> Contain(string expected)
        {
            Fail.When(
                !a.Subject.Contains(expected, StringComparison.Ordinal),
                MessageBuilder.Expected(a.SubjectExpression, $"to contain {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<string> NotContain(string expected)
        {
            Fail.When(
                a.Subject.Contains(expected, StringComparison.Ordinal),
                MessageBuilder.Expected(a.SubjectExpression, $"not to contain {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<string> StartWith(string expected)
        {
            Fail.When(
                !a.Subject.StartsWith(expected, StringComparison.Ordinal),
                MessageBuilder.Expected(a.SubjectExpression, $"to start with {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<string> NotStartWith(string expected)
        {
            Fail.When(
                a.Subject.StartsWith(expected, StringComparison.Ordinal),
                MessageBuilder.Expected(a.SubjectExpression, $"not to start with {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<string> EndWith(string expected)
        {
            Fail.When(
                !a.Subject.EndsWith(expected, StringComparison.Ordinal),
                MessageBuilder.Expected(a.SubjectExpression, $"to end with {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<string> NotEndWith(string expected)
        {
            Fail.When(
                a.Subject.EndsWith(expected, StringComparison.Ordinal),
                MessageBuilder.Expected(a.SubjectExpression, $"not to end with {MessageBuilder.Format(expected)}", a.Subject));
            return a;
        }

        public Assertions<string> Match(string pattern)
        {
            Fail.When(
                !Regex.IsMatch(a.Subject, pattern),
                MessageBuilder.Expected(a.SubjectExpression, $"to match pattern \"{pattern}\"", a.Subject));
            return a;
        }

        public Assertions<string> NotMatch(string pattern)
        {
            Fail.When(
                Regex.IsMatch(a.Subject, pattern),
                MessageBuilder.Expected(a.SubjectExpression, $"not to match pattern \"{pattern}\"", a.Subject));
            return a;
        }

        public Assertions<string> ContainAll(params IEnumerable<string> expected)
        {
            var missing = expected.Where(x => !a.Subject.Contains(x, StringComparison.Ordinal)).ToList();
            Fail.When(
                missing.Count > 0,
                () => MessageBuilder.Expected(a.SubjectExpression, $"to contain all of {MessageBuilder.Format(missing)}", a.Subject));
            return a;
        }

        public Assertions<string> ContainAny(params IEnumerable<string> expected)
        {
            var expectedList = expected as IReadOnlyCollection<string> ?? expected.ToList();
            Fail.When(
                !expectedList.Any(x => a.Subject.Contains(x, StringComparison.Ordinal)),
                () => MessageBuilder.Expected(a.SubjectExpression, $"to contain any of {MessageBuilder.Format(expectedList)}", a.Subject));
            return a;
        }

        public Assertions<string> BeEmpty()
        {
            Fail.When(
                a.Subject.Length != 0,
                MessageBuilder.Expected(a.SubjectExpression, "to be empty", a.Subject));
            return a;
        }

        public Assertions<string> NotBeEmpty()
        {
            Fail.When(
                a.Subject.Length == 0,
                MessageBuilder.Expected(a.SubjectExpression, "not to be empty", MessageBuilder.OmitActual));
            return a;
        }

        public Assertions<string> HaveLength(int expected)
        {
            Fail.When(
                a.Subject.Length != expected,
                () => MessageBuilder.Expected(a.SubjectExpression, $"to have length {expected}", a.Subject.Length));
            return a;
        }

        public Assertions<string> BeEquivalentTo(string expected)
        {
            Fail.When(
                !string.Equals(a.Subject, expected, StringComparison.OrdinalIgnoreCase),
                MessageBuilder.Expected(a.SubjectExpression, $"to be equivalent to {MessageBuilder.Format(expected)} (case-insensitive)", a.Subject));
            return a;
        }

        public Assertions<string> BeNullOrEmpty()
        {
            Fail.When(
                a.Subject.Length != 0,
                MessageBuilder.Expected(a.SubjectExpression, "to be null or empty", a.Subject));
            return a;
        }

        public Assertions<string> NotBeNullOrEmpty()
        {
            Fail.When(
                a.Subject.Length == 0,
                MessageBuilder.Expected(a.SubjectExpression, "not to be null or empty", a.Subject));
            return a;
        }

        public Assertions<string> BeNullOrWhiteSpace()
        {
            Fail.When(
                !string.IsNullOrWhiteSpace(a.Subject),
                MessageBuilder.Expected(a.SubjectExpression, "to be null or whitespace", a.Subject));
            return a;
        }

        public Assertions<string> NotBeNullOrWhiteSpace()
        {
            Fail.When(
                string.IsNullOrWhiteSpace(a.Subject),
                MessageBuilder.Expected(a.SubjectExpression, "not to be null or whitespace", a.Subject));
            return a;
        }
    }
}
