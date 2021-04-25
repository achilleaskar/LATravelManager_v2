using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LATravelManager.UI.Converters
{
    public class NumberWithStringFront : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int decimalValue && parameter is string p)
            {
                
                return p + decimalValue;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;
            if (value is string s && parameter is string p)
            {
                return decimal.Parse(new string(s.Where(c => char.IsDigit(c)).ToArray()));
            }
            return DependencyProperty.UnsetValue;
        }
    }
}