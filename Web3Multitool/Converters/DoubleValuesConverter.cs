using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace Web3Multitool.Converters;

public class DoubleValuesConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            return value == null ? "0" : $"{value:N2}";
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