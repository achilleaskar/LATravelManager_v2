using System;
using System.Globalization;
using System.Windows.Data;
using LATravelManager.Model;

namespace LATravelManager.UI.Converters
{
    public class NotCategoryConverter : IValueConverter
    {
        #region Methods

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is NotificaationType n)
            {
                switch (n)
                {
                    case NotificaationType.CheckIn:
                        return "Check-In";

                    case NotificaationType.Option:
                        return "Option Οργανωμενων";

                    case NotificaationType.PersonalOption:
                        return "Option Ατομικού";

                    case NotificaationType.NoPay:
                        return "Οφειλέτες";

                    default:
                        return "Σφάλμα";
                }
            }
            return "Error";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

        #endregion Methods
    }
}