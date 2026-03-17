namespace ToolBX.AssertBox;

public static class ShouldExtensions
{
    public static Assertions<T> Should<T>(
        this T subject,
        [CallerArgumentExpression(nameof(subject))] string expression = "") =>
        new(subject, expression);

    public static Assertions<Action> Should(
        this Action subject,
        [CallerArgumentExpression(nameof(subject))] string expression = "") =>
        new(subject, expression);

    public static Assertions<Func<Task>> Should(
        this Func<Task> subject,
        [CallerArgumentExpression(nameof(subject))] string expression = "") =>
        new(subject, expression);

}
