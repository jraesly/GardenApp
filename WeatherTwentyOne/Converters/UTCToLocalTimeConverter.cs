using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace WeatherTwentyOne.Converters;


public class UTCToLocalTimeConverter : IValueConverter
{
    // This method converts the UTC timestamp to local time
    // and returns either the whole DateTime or the day of the week as a string.
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Check if the value is a valid integer timestamp
        if (value is int timestamp)
        {
            // Convert the timestamp to a local DateTime
            DateTime dateTime = DateTimeOffset.FromUnixTimeSeconds(timestamp).ToLocalTime().DateTime;

            // Check if the parameter is set to "DayOfWeek"
            if (parameter is string strParameter && strParameter == "DayOfWeek")
            {
                // Return the day of the week as a string (e.g., "Mon", "Tue", etc.)
                return dateTime.ToString("ddd");
            }
            else
            {
                // Return the whole DateTime object
                return dateTime;
            }
        }
        // If the value is not a valid timestamp, return it unmodified
        return value;
    }

    // This method is not implemented, as we don't need to convert back from local time to UTC
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
