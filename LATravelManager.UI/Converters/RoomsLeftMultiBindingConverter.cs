using LATravelManager.Model.Hotels;
using LATravelManager.UI.ViewModel.CategoriesViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace LATravelManager.UI.Converters
{
    public class RoomsLeftMultiBindingConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int freeRooms = 0;
            int count = 0;
            int roomTypeId = 0;
            if (values[0] is CollectionViewGroup item)
            {
                count = item.ItemCount;
                roomTypeId = (item.Name as RoomType).Id;
            }
            if (values[1] is AvailabilitiesList al)
            {
               al.RoomTYpesLeft.TryGetValue(roomTypeId, out freeRooms);
            }
            return "( " + ((freeRooms < count) ? freeRooms : count) + " Προς κράτηση )";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
