using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LATravelManager.UI.Converters
{
    [ValueConversion(typeof(decimal), typeof(string))]
    public class FloadToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            decimal decimalValue = (decimal)value;

            var decimalString = decimalValue.ToString();
            //if (decimalString.Length > 5)
            //{
            //    var t = (decimalString.Length - decimalString.IndexOfAny(new char[] { ',', '.' }));
            //    if (t >= 3)
            //        decimalString = decimalString.Replace(".", "").Replace(",", "");
            //}
            string s = decimalString.IndexOfAny(new char[] { ',', '.' }) >= 0 ? decimalString.TrimEnd('0').TrimEnd('0').TrimEnd('.').TrimEnd(',') : decimalString;
            int indexofcomma = s.IndexOf(',');
            if (indexofcomma <= 0)
            {
                indexofcomma = s.Length;
            }
            if (indexofcomma > 3)
            {
                int firstDot = indexofcomma - 3;
                while (firstDot > 0)
                {
                    s = s.Insert(firstDot, ".");
                    firstDot -= 3;
                }
            }
            if (parameter is string s1 && s1[0] == '0')
                return s;
            return s + " €";
            //return decimalValue > 0 ? decimalValue.ToString() + " €" : "0 €";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string strValue = (value as string).Replace(".", "").Replace(',', '.').Replace("€", "").Replace(" ", "");
            if (!string.IsNullOrEmpty(strValue) && !strValue.EndsWith(".0") && strValue[strValue.Length - 1] != '.' && decimal.TryParse(strValue, NumberStyles.Any, new CultureInfo("en-US"), out var tmpdecimal))
            {
                return decimal.Round(tmpdecimal,2);
            }
            return DependencyProperty.UnsetValue;
        }
    }
}