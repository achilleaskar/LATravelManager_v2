using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LATravelManager.UI.Converters
{
    [ValueConversion(typeof(float), typeof(string))]
    public class FloadToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            float floatType = (float)value;
            return floatType.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = (value as string).Replace(',', '.');
            if (!string.IsNullOrEmpty(strValue) && strValue[strValue.Length - 1] != '.' && float.TryParse(strValue, NumberStyles.Any, new CultureInfo("en-US"), out float tmpFloat))
            {
                return tmpFloat;
            }
            return DependencyProperty.UnsetValue;
        }
    }
}