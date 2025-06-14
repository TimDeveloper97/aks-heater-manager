using System.Globalization;

namespace maui_heater_manager.Converters;

public class TabToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null || parameter == null)
            return Colors.Gray;

        int selectedTab = 0;
        int tabIndex = 0;
        int.TryParse(value.ToString(), out selectedTab);
        int.TryParse(parameter.ToString(), out tabIndex);

        return selectedTab == tabIndex ? Colors.DodgerBlue : Colors.Gray;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
