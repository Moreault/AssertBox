namespace ToolBX.AssertBox;

public sealed class AssertBoxException : Exception
{
    public AssertBoxException(string message) : base(message) { }
}
