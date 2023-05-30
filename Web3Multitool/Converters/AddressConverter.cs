using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace Web3Multitool.Converters;

public class AddressConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            return value == null ? "Not set" : "0x..." + ((string)value)[^8..];
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            return "Invalid";
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}