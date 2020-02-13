using System;
using System.Collections.Generic;
using LATravelManager.Model.BookingData;

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