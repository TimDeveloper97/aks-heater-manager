using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maui_heater_manager.Converters;

class ReverseBoolConverter : IValueConverter
{
    object? IValueConverter.Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    => bool.Parse(value?.ToString() ?? "false") ? false : true;

    object? IValueConverter.ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    => bool.Parse(value?.ToString() ?? "false") ? false : true;
}

