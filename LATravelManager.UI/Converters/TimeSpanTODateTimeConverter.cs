using System;
using System.Globalization;
using System.Windows.Data;

namespace LATravelManager.UI.Converters
{
    public class TimeSpanToDateTimeConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TimeSpan val)
            {
                return new DateTime(val.Ticks);
            }
            return new DateTime();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime val)
            {
                return new TimeSpan(val.Hour, val.Minute, 0);
            }
            return new TimeSpan();
        }

        #endregion Methods
    }
}