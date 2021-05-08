using System;
using System.Globalization;
using System.Windows.Data;

namespace WordNet.Wpf.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class NegateBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}