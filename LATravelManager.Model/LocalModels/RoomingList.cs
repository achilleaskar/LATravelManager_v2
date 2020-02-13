using System.Collections.Generic;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Wrapper;

namespace LATravelManager.Model.LocalModels
{
    public class RoomingList : BaseModel
    {
        public const string hotelPropertyName = nameof(Hotel);

        private HotelWrapper _hotel;

        /// <summary>
        /// Sets and gets the hotel property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public HotelWrapper Hotel
        {
            get
            {
                return _hotel;
            }

            set
            {
                if (_hotel == value)
                {
                    return;
                }

                _hotel = value;
                RaisePropertyChanged(hotelPropertyName);
            }
        }

        public const string ReservationsPropertyName = nameof(Reservations);

        private List<ReservationWrapper> _Reservations = new List<ReservationWrapper>();

        /// <summary>
        /// Sets and gets the Reservations property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public List<ReservationWrapper> Reservations
        {
            get
            {
                return _Reservations;
            }

            set
            {
                if (_Reservations == value)
                {
                    return;
                }

                _Reservations = value;
                RaisePropertyChanged(ReservationsPropertyName);
            }
        }
    }
}