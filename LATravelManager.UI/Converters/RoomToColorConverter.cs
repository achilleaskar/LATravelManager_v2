﻿using LATravelManager.Model.Hotels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace LATravelManager.UI.Converters
{
    public class RoomToColorConverter : IValueConverter
    {

        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Room room)
            {
                //if (room.IsRoomAllotment)
                //{
                //    return new SolidColorBrush(Colors.Blue);
                //}
            }
            return new SolidColorBrush(Colors.Green);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods

    }
}
