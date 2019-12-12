using LATravelManager.Model.BookingData;
using LATravelManager.Model.Hotels;
using LATravelManager.UI.ViewModel.Tabs.TabViewmodels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LATravelManager.UI.Converters
{
    public class GroupToContainsConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int counter = 0;
            var item = value as CollectionViewGroup;
            if (item.Name is Hotel h)
            {
                foreach (CollectionViewGroup p in item.Items)
                {
                    if (p.Name is Period pp)
                    {
                        foreach (CollectionViewGroup r in p.Items)
                        {
                            if (r.Name is RoomType rt)
                            {
                                foreach (roomDetail d in r.Items)
                                {
                                        counter += d.Count;
                                }
                            }
                        }
                    }
                }
            }
            else if (item.Name is Period pp)
            {
                foreach (CollectionViewGroup r in item.Items)
                {
                    if (r.Name is RoomType rt)
                    {
                        foreach (roomDetail d in r.Items)
                        {
                            counter += d.Count;
                        }
                    }
                }
            }
            //var freeRooms = (item.Name as RoomType).freeRooms;
            return counter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}