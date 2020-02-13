using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using LATravelManager.Model.People;

namespace LATravelManager.UI.Converters
{
    public class RemainingToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Counter c && c.UnHandled == 0)
            {
                new SolidColorBrush(Colors.Blue);
            }
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}