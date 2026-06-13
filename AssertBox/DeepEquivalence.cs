namespace ToolBX.AssertBox;

internal static class DeepEquivalence
{
    public static bool AreEquivalent(object? left, object? right, out string? difference, string path = "")
    {
        var visited = new HashSet<(object, object)>(ReferenceEqualityComparer.Instance);
        return AreEquivalent(left, right, out difference, path, visited);
    }

    [UnconditionalSuppressMessage("ReflectionAnalysis", "IL2072",
        Justification = "DeepEquivalence compares objects whose types are known at runtime.")]
    private static bool AreEquivalent(object? left, object? right, out string? difference, string path, HashSet<(object, object)> visited)
    {
        difference = null;

        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
        {
            difference = string.IsNullOrEmpty(path) ? "<root>" : path;
            return false;
        }

        var leftType = left.GetType();
        var rightType = right.GetType();

        if (leftType != rightType && !(left is IEnumerable && right is IEnumerable))
        {
            difference = string.IsNullOrEmpty(path) ? "<root>" : path;
            return false;
        }

        if (leftType.IsPrimitive || left is string || left is decimal || left is DateTime || left is DateTimeOffset || left is Guid || left is TimeSpan || leftType.IsEnum)
        {
            if (left.Equals(right))
                return true;

            difference = string.IsNullOrEmpty(path) ? "<root>" : path;
            return false;
        }

        if (!visited.Add((left, right)))
            return true;

        if (left is IEnumerable leftEnumerable && right is IEnumerable rightEnumerable)
        {
            var leftList = new List<object?>();
            foreach (var item in leftEnumerable)
                leftList.Add(item);

            var rightList = new List<object?>();
            foreach (var item in rightEnumerable)
                rightList.Add(item);

            if (leftList.Count != rightList.Count)
            {
                difference = string.IsNullOrEmpty(path) ? "<root>" : path;
                return false;
            }

            var matched = new bool[rightList.Count];
            foreach (var leftItem in leftList)
            {
                var found = false;
                for (var i = 0; i < rightList.Count; i++)
                {
                    if (matched[i])
                        continue;

                    if (AreEquivalent(leftItem, rightList[i], out _, path, new HashSet<(object, object)>(visited, ReferenceEqualityComparer.Instance)))
                    {
                        matched[i] = true;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    difference = string.IsNullOrEmpty(path) ? "<root>" : path;
                    return false;
                }
            }

            return true;
        }

        var properties = GetPublicProperties(leftType);
        foreach (var prop in properties)
        {
            if (prop.GetIndexParameters().Length > 0)
                continue;

            object? leftValue;
            object? rightValue;
            try
            {
                leftValue = prop.GetValue(left);
                rightValue = prop.GetValue(right);
            }
            catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
            {
                difference = string.IsNullOrEmpty(path) ? prop.Name : $"{path}.{prop.Name}";
                return false;
            }

            var propertyPath = string.IsNullOrEmpty(path) ? prop.Name : $"{path}.{prop.Name}";

            if (!AreEquivalent(leftValue, rightValue, out difference, propertyPath, visited))
                return false;
        }

        return true;
    }

    private static PropertyInfo[] GetPublicProperties(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicProperties)] Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    private sealed class ReferenceEqualityComparer : IEqualityComparer<(object, object)>
    {
        public static readonly ReferenceEqualityComparer Instance = new();

        public bool Equals((object, object) x, (object, object) y) =>
            ReferenceEquals(x.Item1, y.Item1) && ReferenceEquals(x.Item2, y.Item2);

        public int GetHashCode((object, object) obj) =>
            HashCode.Combine(RuntimeHelpers.GetHashCode(obj.Item1), RuntimeHelpers.GetHashCode(obj.Item2));
    }
}
