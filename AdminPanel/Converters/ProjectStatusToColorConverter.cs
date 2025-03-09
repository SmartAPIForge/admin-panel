using System.Globalization;

namespace AdminPanel.Converters;

public class ProjectStatusToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            return status switch
            {
                "GENERATE_SUCCESS" => Colors.Green,
                "GENERATE_FAIL" => Colors.Red,
                "DEPLOY_PENDING" => Colors.Yellow,
                "DEPLOY_SUCCESS" => Colors.Green,
                "DEPLOY_FAIL" => Colors.Red,
                _ => Colors.Red
            };
        }
        return Colors.Red;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}