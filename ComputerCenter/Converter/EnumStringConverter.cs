using System;
using System.Windows.Data;

namespace ComputerCenter.Converter
{
    public class EnumStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    var enumString = Enum.GetName((value.GetType()), value);
                    return enumString;
                }
                return "";
            }
            catch
            {
                return string.Empty;
            }
        }

        // No need to implement converting back on a one-way binding
        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}