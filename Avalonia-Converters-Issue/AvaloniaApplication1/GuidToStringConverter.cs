using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace AvaloniaApplication1
{
    public sealed class GuidToStringConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string str = value?.ToString();
            if (Guid.TryParse(str, out Guid result))
                return result;

            return new BindingNotification(new FormatException("Invalid Guid format"),
                BindingErrorType.DataValidationError);
        }

        public static GuidToStringConverter Instance { get; } = new GuidToStringConverter();
    }
}
