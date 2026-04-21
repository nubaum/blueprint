using System.Globalization;
using System.Windows.Data;

namespace Blueprint.Helpers;

internal class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool boolValue)
        {
            throw new ArgumentException("ExceptionBooleanToVisibilityConverterValueMustBeABoolean");
        }

        return boolValue ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not Visibility visibility)
        {
            throw new ArgumentException("ExceptionBooleanToVisibilityConverterValueMustBeAVisibility");
        }

        return visibility == Visibility.Visible;
    }
}
