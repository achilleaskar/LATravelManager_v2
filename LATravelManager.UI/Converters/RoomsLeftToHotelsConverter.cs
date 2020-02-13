using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using LATravelManager.Model.Hotels;
using LATravelManager.UI.Wrapper;

namespace LATravelManager.UI.Converters
{
    public class RoomsLeftToHotelsConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Hotel> list = new List<Hotel>();
            StringBuilder sb = new StringBuilder();
            var group = value as CollectionViewGroup;
            foreach (RoomWrapper item in group.Items)
            {
                if (!list.Any(h => h.Id == item.Hotel.Id))
                {
                    list.Add(new Hotel { Name = item.Hotel.Name, Id = item.Hotel.Id });
                    list.Last().Rooms.Add(item.Model);
                }
                else
                {
                    list.Where(h => h.Id == item.Hotel.Id).FirstOrDefault().Rooms.Add(item.Model);
                }
            }
            var freeRooms = (group.Name as RoomType).freeRooms;
            sb.Append("(");
            foreach (var h in list)
            {
                sb.Append($"{h.Rooms.Count}-{h.Name} / ");
            }
            sb.Append(")");
            return sb.ToString().Replace(" / )", ")");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}