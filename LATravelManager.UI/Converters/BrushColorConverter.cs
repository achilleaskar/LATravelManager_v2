using LATravelManager.Model.Hotels;
using LATravelManager.UI.Wrapper;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LATravelManager.UI.Converters
{
    public class BrushColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var room = value as RoomWrapper;
            //if (room.)
            //{
            //    return new SolidColorBrush(Colors.Blue);
            //}
            return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}