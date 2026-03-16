namespace ToolBX.AssertBox.Assertions;

public static class GuidAssertions
{
    extension(Assertions<Guid> a)
    {
        public Assertions<Guid> BeEmpty()
        {
            Fail.When(
                a.Subject != Guid.Empty,
                MessageBuilder.Expected(a.SubjectExpression, "to be empty", a.Subject));
            return a;
        }

        public Assertions<Guid> NotBeEmpty()
        {
            Fail.When(
                a.Subject == Guid.Empty,
                MessageBuilder.Expected(a.SubjectExpression, "not to be empty", MessageBuilder.OmitActual));
            return a;
        }
    }
}
