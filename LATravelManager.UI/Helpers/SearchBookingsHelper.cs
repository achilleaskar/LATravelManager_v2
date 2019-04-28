using LATravelManager.Model.BookingData;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Wrapper;
using System;
using System.Threading.Tasks;

namespace LATravelManager.UI.Helpers
{
    public class SearchBookingsHelper
    {
        public SearchBookingsHelper(GenericRepository context)
        {
            Context = context;
        }

        public GenericRepository Context { get; }

        //    public UnitOfWork UOW
        //    {
        //        get
        //        {
        //            return ServiceLocator.Current.GetInstance<UnitOfWork>(Definitions.UnitOfWorkKey);
        //        }
        //    }

        internal async Task DeleteBooking(Booking selectedBooking)
        {
            Context.Delete(selectedBooking);
            await Context.SaveAsync();
        }

        internal async Task DeleteReservation(ReservationWrapper selectedReservation)
        {
            if (selectedReservation.Booking.ReservationsInBooking.Count == 1)
            {
                Context.DeletePayments(selectedReservation.Booking);
                Context.Delete(selectedReservation.Booking);
            }
            else
                Context.Delete(selectedReservation.Model);
            await Context.SaveAsync();
        }

        internal DateTime GetDateLimit(string parameter)
        {
            int.TryParse(parameter, out int i);
            DateTime dateLimit;
            switch (i)
            {
                case 1:
                    dateLimit = DateTime.Today;
                    break;

                case 7:
                    dateLimit = DateTime.Today.StartOfWeek(DayOfWeek.Monday);
                    break;

                case 30:
                    dateLimit = DateTime.Today.AddDays(-30);
                    break;

                default:
                    dateLimit = new DateTime();

                    break;
            }
            return dateLimit;
        }
    }
}