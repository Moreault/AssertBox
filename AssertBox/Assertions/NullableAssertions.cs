namespace ToolBX.AssertBox.Assertions;

public static class NullableAssertions
{
    extension<T>(Assertions<T?> a) where T : struct
    {
        public Assertions<T?> HaveValue()
        {
            Fail.When(
                !a.Subject.HasValue,
                MessageBuilder.Expected(a.SubjectExpression, "to have a value", "<null>"));
            return a;
        }

        public Assertions<T?> NotBeNull()
        {
            Fail.When(
                !a.Subject.HasValue,
                MessageBuilder.Expected(a.SubjectExpression, "not to be <null>", "<null>"));
            return a;
        }

        public Assertions<T?> NotHaveValue()
        {
            Fail.When(
                a.Subject.HasValue,
                MessageBuilder.Expected(a.SubjectExpression, "not to have a value", a.Subject));
            return a;
        }

        public Assertions<T?> BeNull()
        {
            Fail.When(
                a.Subject.HasValue,
                MessageBuilder.Expected(a.SubjectExpression, "to be <null>", a.Subject));
            return a;
        }
    }
}
