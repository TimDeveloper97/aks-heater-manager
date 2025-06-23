using System;
using System.Globalization;
using Xamarin.Forms;

namespace maui_heater_manager.Converters
{
    public class TabToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Color.Gray;

            int selectedTab = 0;
            int tabIndex = 0;
            int.TryParse(value.ToString(), out selectedTab);
            int.TryParse(parameter.ToString(), out tabIndex);

            return selectedTab == tabIndex ? Color.FromHex("#512BD4") : Color.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}