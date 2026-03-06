namespace ToolBX.AssertBox.Assertions;

public static class BooleanAssertions
{
    extension(Assertions<bool> a)
    {
        public Assertions<bool> BeTrue()
        {
            Fail.When(!a.Subject, MessageBuilder.Expected(a.SubjectExpression, "to be true", false));
            return a;
        }

        public Assertions<bool> BeFalse()
        {
            Fail.When(a.Subject, MessageBuilder.Expected(a.SubjectExpression, "to be false", true));
            return a;
        }
    }
}
