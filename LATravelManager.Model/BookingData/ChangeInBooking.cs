using LATravelManager.Model.People;
using System;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.BookingData
{
    public class ChangeInBooking : BaseModel
    {


        public ChangeInBooking()
        {
        }

        public Booking Booking { get; set; }
        public User User { get; set; }




        private DateTime _Date;


        public DateTime Date
        {
            get
            {
                return _Date;
            }

            set
            {
                if (_Date == value)
                {
                    return;
                }

                _Date = value;
                RaisePropertyChanged();
            }
        }




        private string _Description;

        [StringLength(500)]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (_Description == value)
                {
                    return;
                }

                _Description = value;
                RaisePropertyChanged();
            }
        }

    }
}