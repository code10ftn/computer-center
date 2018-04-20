using System;
using System.Windows.Data;

namespace ComputerCenter.Converter
{
    public class BooleanStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null && (bool)value ? "Yes" : "No";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return (string)value == "Yes";
        }
    }
}