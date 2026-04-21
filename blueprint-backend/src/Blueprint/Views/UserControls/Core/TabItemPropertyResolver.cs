using System.Reflection;

namespace Blueprint.Views.UserControls.Core;

internal static class TabItemPropertyResolver
{
    public static object? GetValue(object item, string? memberPath)
    {
        if (item == null || string.IsNullOrWhiteSpace(memberPath))
        {
            return null;
        }

        object? current = item;

        foreach (string member in memberPath.Split('.'))
        {
            if (current == null)
            {
                return null;
            }

            PropertyInfo? property = current.GetType().GetProperty(
                member,
                BindingFlags.Instance | BindingFlags.Public);

            if (property == null)
            {
                return null;
            }

            current = property.GetValue(current);
        }

        return current;
    }

    public static string GetString(object item, string? memberPath, string defaultValue = "")
        => GetValue(item, memberPath) as string ?? defaultValue;

    public static bool GetBool(object item, string? memberPath, bool defaultValue = false)
    {
        object? value = GetValue(item, memberPath);

        return value switch
        {
            bool boolValue => boolValue,
            null => defaultValue,
            _ => defaultValue
        };
    }
}
