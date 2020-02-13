using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using LATravelManager.Model.BookingData;

namespace LATravelManager.UI.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Payment paym)
            {
                if (paym.Checked == true)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                if (paym.Outgoing)
                {
                    return new SolidColorBrush(Colors.Blue);
                }
                if (paym.Checked == null)
                {
                    return new SolidColorBrush(Colors.Transparent);
                }
            }
            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}