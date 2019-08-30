using LATravelManager.Model.BookingData;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LATravelManager.UI.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Payment paym = value as Payment;
            if (paym != null)
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