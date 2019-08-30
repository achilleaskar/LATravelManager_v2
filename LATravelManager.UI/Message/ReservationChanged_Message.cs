using LATravelManager.Model.BookingData;

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