using System;
using System.Globalization;
using System.Windows.Data;
using LATravelManager.Model.Hotels;
using LATravelManager.UI.ViewModel.CategoriesViewModels;

namespace LATravelManager.UI.Converters
{
    public class RoomsLeftToVisibiliyConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int freeRooms = 0;
            int roomTypeId = 0;
            if (values[0] is CollectionViewGroup item)
            {
                roomTypeId = (item.Name as RoomType).Id;
            }
            if (values[1] is AvailabilitiesList al)
            {
                //al.RoomTYpesLeft.TryGetValue(roomTypeId, out freeRooms);
            }
            return freeRooms > 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}