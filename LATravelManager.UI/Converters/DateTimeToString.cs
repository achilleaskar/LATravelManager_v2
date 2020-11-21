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
            if (value is TimeSpan ts)
            {
                return ts.ToString(@"hh\:mm");
            }
            if (value == null)
            {
                return "";
            }
            return "Error";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string date && parameter is string par)
            {
                if (DateTime.TryParseExact(date, par, CultureInfo.CurrentUICulture, DateTimeStyles.None, out DateTime datet))
                {
                    return datet;
                }
                if (par is string s && s != null && s.Length == 1 && DateTime.TryParseExact(date, "HH:mm", CultureInfo.InvariantCulture,
                                              DateTimeStyles.None, out DateTime datets))
                {
                    return datets.TimeOfDay;
                }
            }
            return null;
        }

        #endregion Methods
    }
}