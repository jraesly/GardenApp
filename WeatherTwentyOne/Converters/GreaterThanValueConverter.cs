using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace WeatherTwentyOne.Converters
{
    public class GreaterThanValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double.TryParse(parameter?.ToString(), out double threshold);

            if (value is double val && threshold != 0)
            {
                return val > threshold;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
