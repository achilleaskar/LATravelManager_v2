using LATravelManager.Model.Hotels;
using LATravelManager.UI.Wrapper;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace LATravelManager.UI.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var check = value as bool?;
            if (check == true)
            {
                return new SolidColorBrush(Colors.Green);
            }
            if (check==null)
            {
                return new SolidColorBrush(Colors.Transparent);

            }
            return new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}