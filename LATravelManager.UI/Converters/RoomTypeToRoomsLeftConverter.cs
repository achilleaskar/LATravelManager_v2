using System;
using System.Globalization;
using System.Windows.Data;
using LATravelManager.Model.Hotels;

namespace LATravelManager.UI.Converters
{
    public class RoomTypeToRoomsLeftConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as CollectionViewGroup;
            var freeRooms = (item.Name as RoomType).freeRooms;
            return "( " + ((freeRooms < item.ItemCount) ? freeRooms : item.ItemCount) + " Προς κράτηση )";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}