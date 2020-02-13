using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LATravelManager.UI.Converters
{
    public class BoolToRedGreenConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                if (b)
                    return new SolidColorBrush(Colors.Green);

                return new SolidColorBrush(Colors.Red);
            }

            if (value is decimal d)
            {
                if (d < 1)
                    return new SolidColorBrush(Colors.Green);

                return new SolidColorBrush(Colors.Red);
            }
            return new SolidColorBrush(Colors.Blue);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}