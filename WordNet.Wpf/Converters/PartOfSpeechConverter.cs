using System;
using System.Globalization;
using System.Windows.Data;
using WordNet.Data.Model;

namespace WordNet.Wpf.Converters
{
    public class PartOfSpeechConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is PartOfSpeech)) return null;
            return value switch
            {
                PartOfSpeech.AdjectiveSatellite => PartOfSpeech.Adjective + "s",
                _ => value + "s",
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}