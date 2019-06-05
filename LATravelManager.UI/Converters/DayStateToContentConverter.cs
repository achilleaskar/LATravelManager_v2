using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.Converters
{
    public class DayStateToContentConverter : IValueConverter
    {

        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((DayStateEnum)value)
            {
                case DayStateEnum.FirstDay:
                    return "<";

                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods

    }
}
