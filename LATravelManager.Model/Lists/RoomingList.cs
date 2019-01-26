using LATravelManager.BaseTypes;
using System.Collections.Generic;

namespace LATravelManager.Models
{
    public class RoomingList: BaseModel
    {


        public const string hotelPropertyName = nameof(Hotel);

        private Hotel _hotel     ;

        /// <summary>
        /// Sets and gets the hotel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Hotel Hotel
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

        private List<Reservation> _Reservations = new List<Reservation>();

        /// <summary>
        /// Sets and gets the Reservations property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<Reservation> Reservations
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
