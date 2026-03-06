namespace ToolBX.AssertBox;

public static class Fail
{
    public static void When([DoesNotReturnIf(true)] bool condition, string message)
    {
        if (condition)
            throw new AssertBoxException(message);
    }

    public static void When([DoesNotReturnIf(true)] bool condition, Func<string> messageFactory)
    {
        if (condition)
            throw new AssertBoxException(messageFactory());
    }

    [DoesNotReturn]
    [StackTraceHidden]
    public static void With(string message) => throw new AssertBoxException(message);
}
