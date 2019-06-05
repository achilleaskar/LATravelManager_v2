using LATravelManager.Model.BookingData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.UI.Message
{
    public class ReservationChanged_Message
    {
        public ReservationChanged_Message(Booking booking)
        {
            Booking = booking;
        }

        public Booking Booking { get; }
    }


}
