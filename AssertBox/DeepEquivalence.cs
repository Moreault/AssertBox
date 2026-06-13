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

        if (IsNumeric(leftType) && IsNumeric(rightType))
        {
            if (NumericEquals(left, right))
                return true;

            difference = string.IsNullOrEmpty(path) ? "<root>" : path;
            return false;
        }

        if (IsSimple(leftType) || IsSimple(rightType))
        {
            if (leftType == rightType && left.Equals(right))
                return true;

            difference = string.IsNullOrEmpty(path) ? "<root>" : path;
            return false;
        }

        if (left is IEnumerable != right is IEnumerable)
        {
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

        var sameType = leftType == rightType;
        var rightProperties = sameType ? null : GetPublicProperties(rightType).ToDictionary(p => p.Name);
        var properties = GetPublicProperties(leftType);
        foreach (var prop in properties)
        {
            if (prop.GetIndexParameters().Length > 0)
                continue;

            var propertyPath = string.IsNullOrEmpty(path) ? prop.Name : $"{path}.{prop.Name}";

            PropertyInfo rightProp;
            if (sameType)
            {
                rightProp = prop;
            }
            else if (!rightProperties!.TryGetValue(prop.Name, out rightProp!))
            {
                difference = propertyPath;
                return false;
            }

            object? leftValue;
            object? rightValue;
            try
            {
                leftValue = prop.GetValue(left);
            }
            catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
            {
                if (Throws(rightProp, right, ex.GetType()))
                    continue;
                difference = propertyPath;
                return false;
            }

            try
            {
                rightValue = rightProp.GetValue(right);
            }
            catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
            {
                difference = propertyPath;
                return false;
            }

            if (!AreEquivalent(leftValue, rightValue, out difference, propertyPath, visited))
                return false;
        }

        return true;
    }

    private static bool IsNumeric(Type type) =>
        type == typeof(byte) || type == typeof(sbyte) ||
        type == typeof(short) || type == typeof(ushort) ||
        type == typeof(int) || type == typeof(uint) ||
        type == typeof(long) || type == typeof(ulong) ||
        type == typeof(nint) || type == typeof(nuint) ||
        type == typeof(float) || type == typeof(double) || type == typeof(decimal);

    private static bool IsSimple(Type type) =>
        type.IsPrimitive || type.IsEnum ||
        type == typeof(string) || type == typeof(decimal) ||
        type == typeof(DateTime) || type == typeof(DateTimeOffset) ||
        type == typeof(DateOnly) || type == typeof(TimeOnly) ||
        type == typeof(Guid) || type == typeof(TimeSpan);

    private static bool NumericEquals(object left, object right)
    {
        if (left is float or double || right is float or double)
            return Convert.ToDouble(left, CultureInfo.InvariantCulture).Equals(Convert.ToDouble(right, CultureInfo.InvariantCulture));

        try
        {
            return Convert.ToDecimal(left, CultureInfo.InvariantCulture) == Convert.ToDecimal(right, CultureInfo.InvariantCulture);
        }
        catch (OverflowException)
        {
            return false;
        }
    }

    private static bool Throws(PropertyInfo prop, object target, Type exceptionType)
    {
        try
        {
            prop.GetValue(target);
            return false;
        }
        catch (Exception ex) when (ex is not OutOfMemoryException and not StackOverflowException)
        {
            return ex.GetType() == exceptionType;
        }
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
