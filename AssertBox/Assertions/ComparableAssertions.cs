namespace ToolBX.AssertBox.Assertions;

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

    extension(Assertions<byte> a)
    {
        public Assertions<byte> BeGreaterThan(byte expected)
        {
            Fail.When(a.Subject.CompareTo(expected) <= 0, MessageBuilder.Expected(a.SubjectExpression, $"to be greater than {expected}", a.Subject));
            return a;
        }

        public Assertions<byte> BeGreaterThanOrEqualTo(byte expected)
        {
            Fail.When(a.Subject.CompareTo(expected) < 0, MessageBuilder.Expected(a.SubjectExpression, $"to be greater than or equal to {expected}", a.Subject));
            return a;
        }

        public Assertions<byte> BeLessThan(byte expected)
        {
            Fail.When(a.Subject.CompareTo(expected) >= 0, MessageBuilder.Expected(a.SubjectExpression, $"to be less than {expected}", a.Subject));
            return a;
        }

        public Assertions<byte> BeLessThanOrEqualTo(byte expected)
        {
            Fail.When(a.Subject.CompareTo(expected) > 0, MessageBuilder.Expected(a.SubjectExpression, $"to be less than or equal to {expected}", a.Subject));
            return a;
        }

        public Assertions<byte> BeInRange(byte min, byte max)
        {
            Fail.When(a.Subject.CompareTo(min) < 0 || a.Subject.CompareTo(max) > 0, MessageBuilder.Expected(a.SubjectExpression, $"to be in range [{min}, {max}]", a.Subject));
            return a;
        }
    }

    extension(Assertions<sbyte> a)
    {
        public Assertions<sbyte> BeGreaterThan(sbyte expected)
        {
            Fail.When(a.Subject.CompareTo(expected) <= 0, MessageBuilder.Expected(a.SubjectExpression, $"to be greater than {expected}", a.Subject));
            return a;
        }

        public Assertions<sbyte> BeGreaterThanOrEqualTo(sbyte expected)
        {
            Fail.When(a.Subject.CompareTo(expected) < 0, MessageBuilder.Expected(a.SubjectExpression, $"to be greater than or equal to {expected}", a.Subject));
            return a;
        }

        public Assertions<sbyte> BeLessThan(sbyte expected)
        {
            Fail.When(a.Subject.CompareTo(expected) >= 0, MessageBuilder.Expected(a.SubjectExpression, $"to be less than {expected}", a.Subject));
            return a;
        }

        public Assertions<sbyte> BeLessThanOrEqualTo(sbyte expected)
        {
            Fail.When(a.Subject.CompareTo(expected) > 0, MessageBuilder.Expected(a.SubjectExpression, $"to be less than or equal to {expected}", a.Subject));
            return a;
        }

        public Assertions<sbyte> BeInRange(sbyte min, sbyte max)
        {
            Fail.When(a.Subject.CompareTo(min) < 0 || a.Subject.CompareTo(max) > 0, MessageBuilder.Expected(a.SubjectExpression, $"to be in range [{min}, {max}]", a.Subject));
            return a;
        }
    }

    extension(Assertions<short> a)
    {
        public Assertions<short> BeGreaterThan(short expected)
        {
            Fail.When(a.Subject.CompareTo(expected) <= 0, MessageBuilder.Expected(a.SubjectExpression, $"to be greater than {expected}", a.Subject));
            return a;
        }

        public Assertions<short> BeGreaterThanOrEqualTo(short expected)
        {
            Fail.When(a.Subject.CompareTo(expected) < 0, MessageBuilder.Expected(a.SubjectExpression, $"to be greater than or equal to {expected}", a.Subject));
            return a;
        }

        public Assertions<short> BeLessThan(short expected)
        {
            Fail.When(a.Subject.CompareTo(expected) >= 0, MessageBuilder.Expected(a.SubjectExpression, $"to be less than {expected}", a.Subject));
            return a;
        }

        public Assertions<short> BeLessThanOrEqualTo(short expected)
        {
            Fail.When(a.Subject.CompareTo(expected) > 0, MessageBuilder.Expected(a.SubjectExpression, $"to be less than or equal to {expected}", a.Subject));
            return a;
        }

        public Assertions<short> BeInRange(short min, short max)
        {
            Fail.When(a.Subject.CompareTo(min) < 0 || a.Subject.CompareTo(max) > 0, MessageBuilder.Expected(a.SubjectExpression, $"to be in range [{min}, {max}]", a.Subject));
            return a;
        }
    }

    extension(Assertions<uint> a)
    {
        public Assertions<uint> BeGreaterThan(uint expected)
        {
            Fail.When(a.Subject.CompareTo(expected) <= 0, MessageBuilder.Expected(a.SubjectExpression, $"to be greater than {expected}", a.Subject));
            return a;
        }

        public Assertions<uint> BeGreaterThanOrEqualTo(uint expected)
        {
            Fail.When(a.Subject.CompareTo(expected) < 0, MessageBuilder.Expected(a.SubjectExpression, $"to be greater than or equal to {expected}", a.Subject));
            return a;
        }

        public Assertions<uint> BeLessThan(uint expected)
        {
            Fail.When(a.Subject.CompareTo(expected) >= 0, MessageBuilder.Expected(a.SubjectExpression, $"to be less than {expected}", a.Subject));
            return a;
        }

        public Assertions<uint> BeLessThanOrEqualTo(uint expected)
        {
            Fail.When(a.Subject.CompareTo(expected) > 0, MessageBuilder.Expected(a.SubjectExpression, $"to be less than or equal to {expected}", a.Subject));
            return a;
        }

        public Assertions<uint> BeInRange(uint min, uint max)
        {
            Fail.When(a.Subject.CompareTo(min) < 0 || a.Subject.CompareTo(max) > 0, MessageBuilder.Expected(a.SubjectExpression, $"to be in range [{min}, {max}]", a.Subject));
            return a;
        }
    }

    extension(Assertions<ulong> a)
    {
        public Assertions<ulong> BeGreaterThan(ulong expected)
        {
            Fail.When(a.Subject.CompareTo(expected) <= 0, MessageBuilder.Expected(a.SubjectExpression, $"to be greater than {expected}", a.Subject));
            return a;
        }

        public Assertions<ulong> BeGreaterThanOrEqualTo(ulong expected)
        {
            Fail.When(a.Subject.CompareTo(expected) < 0, MessageBuilder.Expected(a.SubjectExpression, $"to be greater than or equal to {expected}", a.Subject));
            return a;
        }

        public Assertions<ulong> BeLessThan(ulong expected)
        {
            Fail.When(a.Subject.CompareTo(expected) >= 0, MessageBuilder.Expected(a.SubjectExpression, $"to be less than {expected}", a.Subject));
            return a;
        }

        public Assertions<ulong> BeLessThanOrEqualTo(ulong expected)
        {
            Fail.When(a.Subject.CompareTo(expected) > 0, MessageBuilder.Expected(a.SubjectExpression, $"to be less than or equal to {expected}", a.Subject));
            return a;
        }

        public Assertions<ulong> BeInRange(ulong min, ulong max)
        {
            Fail.When(a.Subject.CompareTo(min) < 0 || a.Subject.CompareTo(max) > 0, MessageBuilder.Expected(a.SubjectExpression, $"to be in range [{min}, {max}]", a.Subject));
            return a;
        }
    }

    extension(Assertions<ushort> a)
    {
        public Assertions<ushort> BeGreaterThan(ushort expected)
        {
            Fail.When(a.Subject.CompareTo(expected) <= 0, MessageBuilder.Expected(a.SubjectExpression, $"to be greater than {expected}", a.Subject));
            return a;
        }

        public Assertions<ushort> BeGreaterThanOrEqualTo(ushort expected)
        {
            Fail.When(a.Subject.CompareTo(expected) < 0, MessageBuilder.Expected(a.SubjectExpression, $"to be greater than or equal to {expected}", a.Subject));
            return a;
        }

        public Assertions<ushort> BeLessThan(ushort expected)
        {
            Fail.When(a.Subject.CompareTo(expected) >= 0, MessageBuilder.Expected(a.SubjectExpression, $"to be less than {expected}", a.Subject));
            return a;
        }

        public Assertions<ushort> BeLessThanOrEqualTo(ushort expected)
        {
            Fail.When(a.Subject.CompareTo(expected) > 0, MessageBuilder.Expected(a.SubjectExpression, $"to be less than or equal to {expected}", a.Subject));
            return a;
        }

        public Assertions<ushort> BeInRange(ushort min, ushort max)
        {
            Fail.When(a.Subject.CompareTo(min) < 0 || a.Subject.CompareTo(max) > 0, MessageBuilder.Expected(a.SubjectExpression, $"to be in range [{min}, {max}]", a.Subject));
            return a;
        }
    }
}
