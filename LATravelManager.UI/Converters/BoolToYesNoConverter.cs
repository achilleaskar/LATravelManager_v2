using System;
using System.Globalization;
using System.Windows.Data;

namespace LATravelManager.UI.Converters
{
    public class BoolToYesNoConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                if (b)
                    return "NAI";
                return "OXI";
            }
            return "ERROR";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}