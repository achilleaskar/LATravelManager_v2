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

        //public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    if (value == null)
        //        return "err";
        //    CollectionViewGroup hotel = value as CollectionViewGroup;
        //    ReadOnlyObservableCollection<object> roomtypes = hotel.Items;
        //    foreach (var rt in roomtypes)
        //    {
        //        var x = rt;
        //    }
        //    return "( ";// + ((freeRooms < item.ItemCount) ? freeRooms : item.ItemCount) + " Προς κράτηση )";
        //}

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}