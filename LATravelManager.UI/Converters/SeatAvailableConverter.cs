using System;
using System.Globalization;
using System.Windows.Data;
using LATravelManager.Model.Lists;

namespace LATravelManager.UI.Converters
{
    public class SeatAvailableConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Seat seat)
            {
                if (seat.Customer == null && seat.Number > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}