using LATravelManager.Models;
using System;

namespace LATravelManager.UI.Helpers
{
    public class SearchBookingsHelper
    {
        public UnitOfWork UOW
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UnitOfWork>(Definitions.UnitOfWorkKey);
            }
        }

        internal void DeleteBooking(Booking selectedBooking)
        {
            UOW.GenericRepository.Delete(selectedBooking);
            UOW.Complete();
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