using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace maui_heater_manager.Converters;

public class PointToRectConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length >= 3 &&
            values[0] is double x &&
            values[1] is double y &&
            values[2] is double radius)
        {
            double diameter = radius * 2;
            return new Rect(x - radius, y - radius, diameter, diameter);
        }
        return new Rect();
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
