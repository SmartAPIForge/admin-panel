using System.Globalization;

namespace AdminPanel.Converters;

public class RoleToTextConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is long role)
        {
            return role switch
            {
                0 => "Админ",
                1 => "Пользователь",
                _ => "Неизвестная роль"
            };
        }
        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}