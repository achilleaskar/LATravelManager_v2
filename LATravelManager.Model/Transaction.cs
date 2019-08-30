using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using System;

namespace LATravelManager.Model
{
    public class Transaction : EditTracker
    {
        #region Fields

        private decimal _Amount;
        private Booking _Booking;
        private DateTime _CheckIn;
        private DateTime _CheckOut;
        private int _Code;

        private DateTime _Date;

        private string _Description;

        private User _PaymentTo;

        private User _User;

        #endregion Fields

        #region Properties

        public decimal Amount
        {
            get
            {
                return _Amount;
            }

            set
            {
                if (_Amount == value)
                {
                    return;
                }

                _Amount = value;
                RaisePropertyChanged();
            }
        }

        public Booking Booking
        {
            get
            {
                return _Booking;
            }

            set
            {
                if (_Booking == value)
                {
                    return;
                }

                _Booking = value;
                RaisePropertyChanged();
            }
        }

        public DateTime CheckIn
        {
            get
            {
                return _CheckIn;
            }

            set
            {
                if (_CheckIn == value)
                {
                    return;
                }

                _CheckIn = value;
                RaisePropertyChanged();
            }
        }

        public DateTime CheckOut
        {
            get
            {
                return _CheckOut;
            }

            set
            {
                if (_CheckOut == value)
                {
                    return;
                }

                _CheckOut = value;
                RaisePropertyChanged();
            }
        }

        public int Code
        {
            get
            {
                return _Code;
            }

            set
            {
                if (_Code == value)
                {
                    return;
                }

                _Code = value;
                RaisePropertyChanged();
            }
        }

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

        public User PaymentTo
        {
            get
            {
                return _PaymentTo;
            }

            set
            {
                if (_PaymentTo == value)
                {
                    return;
                }

                _PaymentTo = value;
                RaisePropertyChanged();
            }
        }

        public User User
        {
            get
            {
                return _User;
            }

            set
            {
                if (_User == value)
                {
                    return;
                }

                _User = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}