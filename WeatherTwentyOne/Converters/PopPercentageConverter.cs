using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace WeatherTwentyOne.Converters
{
    public class PopPercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double popValue && popValue > 0.001)
            {
                return $"{popValue * 100:F0}%";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
