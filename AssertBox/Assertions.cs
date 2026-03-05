namespace AssertBox;

public readonly record struct Assertions<T>(T Subject, string SubjectExpression);
