using System;
using System.Globalization;
using System.Windows.Data;

namespace LATravelManager.UI.Converters
{
    public class DateTimeToString : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime val && parameter is string par)
            {
                return val.ToString(par);
            }
            return "Error";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime datet;
            if (value is string date && parameter is string par)
            {
                if (!DateTime.TryParseExact(date, par, CultureInfo.CurrentUICulture, DateTimeStyles.None, out datet))
                {
                    return null;
                }
                return datet;
            }
            return null;
        }

        #endregion Methods
    }
}