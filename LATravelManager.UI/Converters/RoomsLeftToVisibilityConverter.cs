using LATravelManager.Model.Hotels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LATravelManager.UI.Converters
{
   public class RoomsLeftToVisibilityConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = value as CollectionViewGroup;
            var freeRooms = (item.Name as RoomType).freeRooms;
            return (freeRooms > 0);
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();

    }
}
