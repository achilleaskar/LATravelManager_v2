using System;
using System.Globalization;
using System.Windows.Data;
using LATravelManager.UI.Wrapper;

namespace LATravelManager.UI.Converters
{
    public class RoomTypeToLocalNoteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is CollectionViewGroup cvg && cvg.ItemCount == 1 ? (cvg.Items[0] is RoomWrapper rw) ? rw.LocalNote : "" : "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();
    }
}