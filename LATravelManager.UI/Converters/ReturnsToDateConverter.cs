using System;
using System.Globalization;
using System.Windows.Data;
using LATravelManager.Model.Wrapper;

namespace LATravelManager.UI.Converters
{
    public class ReturnsToDateConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ReservationWrapper val && val.Returns && val.CheckOut.Year > 2000)
            {
                return val.CheckOut.ToString("dd/MM");
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}