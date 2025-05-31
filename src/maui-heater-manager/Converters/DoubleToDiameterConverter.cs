using System.Globalization;

namespace maui_heater_manager.Converters;

public class DoubleToDiameterConverter : IValueConverter
{
    object? IValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    => value is double d ? d * 2 : value;

    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    => value is double d ? d / 2 : value;
}

