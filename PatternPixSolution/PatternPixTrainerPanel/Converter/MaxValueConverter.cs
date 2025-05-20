using System;
using System.Globalization;
using System.Windows.Data;

namespace PatternPixTrainerPanel.Converter
{
    public class MaxValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Add a buffer to the maximum value (e.g., 20% more)
            if (value is double maxValue)
            {
                return maxValue * 1.2;
            }

            return 10; // Default fallback value
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}