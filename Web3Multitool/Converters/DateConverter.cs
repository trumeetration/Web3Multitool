using System;
using System.Globalization;
using System.Windows.Data;

namespace Web3Multitool.Converters;

public class DateConverter :  IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return (DateTime)value! == DateTime.MinValue ? "-" : value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}