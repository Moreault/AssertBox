namespace ToolBX.AssertBox.Assertions;

public static class EnumAssertions
{
    extension<T>(Assertions<T> a) where T : struct, Enum
    {
        public Assertions<T> BeDefined()
        {
            Fail.When(
                !Enum.IsDefined(a.Subject),
                MessageBuilder.Expected(a.SubjectExpression, "to be a defined enum value", a.Subject));
            return a;
        }

        public Assertions<T> NotBeDefined()
        {
            Fail.When(
                Enum.IsDefined(a.Subject),
                MessageBuilder.Expected(a.SubjectExpression, "not to be a defined enum value", a.Subject));
            return a;
        }

        public Assertions<T> HaveFlag(T expected)
        {
            Fail.When(
                !a.Subject.HasFlag(expected),
                MessageBuilder.Expected(a.SubjectExpression, $"to have flag {expected}", a.Subject));
            return a;
        }

        public Assertions<T> NotHaveFlag(T expected)
        {
            Fail.When(
                a.Subject.HasFlag(expected),
                MessageBuilder.Expected(a.SubjectExpression, $"not to have flag {expected}", a.Subject));
            return a;
        }
    }
}
