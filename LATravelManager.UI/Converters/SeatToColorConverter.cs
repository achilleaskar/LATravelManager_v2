using LATravelManager.Model.Lists;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LATravelManager.UI.Converters
{
    public class SeatToColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Seat seat)
            {
                if (seat.SeatType == Model.SeatType.Normal)
                {
                    if (seat.Customer!=null)
                    {
                        return seat.Customer.RoomColor;
                    }
                    return new SolidColorBrush(Colors.White);
                }
                if (seat.SeatType == Model.SeatType.Leader || seat.SeatType == Model.SeatType.Driver)
                {
                    return new SolidColorBrush(Colors.LightYellow);
                }
                if (seat.SeatType == Model.SeatType.Road)
                {
                    return new SolidColorBrush(Colors.LightGray);
                }
                if (seat.SeatType == Model.SeatType.Door)
                {
                    return new SolidColorBrush(Colors.DarkGray);
                }
            }
            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}