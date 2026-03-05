namespace AssertBox;

public static class MessageBuilder
{
    public static string Expected(string subjectExpression, string expectation, object? actual = null)
    {
        var sb = new StringBuilder();
        sb.Append("Expected ");
        sb.Append(subjectExpression);
        sb.Append(' ');
        sb.Append(expectation);
        if (actual is not Omitted)
        {
            sb.Append(", but found ");
            sb.Append(Format(actual));
        }
        sb.Append('.');
        return sb.ToString();
    }

    public static string Format(object? value) => value switch
    {
        null => "<null>",
        string s => $"\"{s}\"",
        bool b => b ? "true" : "false",
        char c => $"'{c}'",
        IEnumerable enumerable => FormatEnumerable(enumerable),
        _ => value.ToString() ?? "<null>"
    };

    private static string FormatEnumerable(IEnumerable enumerable)
    {
        var sb = new StringBuilder();
        sb.Append('[');
        var first = true;
        var count = 0;
        foreach (var item in enumerable)
        {
            if (!first) sb.Append(", ");
            if (count >= 10)
            {
                sb.Append("...");
                break;
            }
            sb.Append(Format(item));
            first = false;
            count++;
        }
        sb.Append(']');
        return sb.ToString();
    }

    private readonly struct Omitted;

    public static object OmitActual { get; } = new Omitted();
}
