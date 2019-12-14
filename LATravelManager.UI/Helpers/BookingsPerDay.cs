using LATravelManager.Model.BookingData;
using LATravelManager.Model.Excursions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.Helpers
{
    public partial class DocumentsManagement
    {
        #region Classes

        public class BookingsPerDay
        {
            #region Constructors

            public BookingsPerDay(DateTime dateTime)
            {
                Bookings = new List<Booking>();
                Date = dateTime;
            }

            #endregion Constructors

            #region Fields

            public List<Booking> Bookings;
            public DateTime Date;

            #endregion Fields
        }

        
        #endregion Classes
    }
}