using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using LATravelManager.Model;
using static LATravelManager.UI.Helpers.Definitions;

namespace LATravelManager.UI.Converters
{
    public class DictionaryToCategoryConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HotelCategoryEnum e)
            {
                return HotelCategoryDictionary[e];
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
                return ((KeyValuePair<HotelCategoryEnum, string>)value).Key;
            return null;
        }
    }
}