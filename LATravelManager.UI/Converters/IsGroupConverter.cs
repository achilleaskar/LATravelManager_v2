using LATravelManager.Model.Wrapper;
using System;
using System.Globalization;
using System.Windows.Data;

namespace LATravelManager.UI.Converters
{
    public class IsGroupConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BookingWrapper booking = value as BookingWrapper;
            if (booking.Excursion.ExcursionType.Category == Model.Enums.ExcursionTypeEnum.Group)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}