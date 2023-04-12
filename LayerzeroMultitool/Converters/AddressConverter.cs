using System;
using System.Globalization;
using System.Windows.Data;

namespace LayerzeroMultitool.Converters;

public class AddressConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        return value == null ? "Not set" : "0x..." + ((string)value)[^8..];
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}