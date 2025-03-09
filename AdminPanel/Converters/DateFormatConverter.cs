using System.Globalization;

namespace AdminPanel.Converters;

public class DateFormatConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime date)
            return date.ToString("dd.MM.yyyy");
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (DateTime.TryParseExact(value?.ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result))
            return result;
        return DateTime.MinValue;
    }
}