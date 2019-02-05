using LATravelManager.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class HotelInfo : BaseModel
    {
        #region Constructors

        public HotelInfo()
        {
        }

        #endregion Constructors

        #region Fields

        public const string CheckInPropertyName = nameof(CheckIn);
        public const string CheckOutPropertyName = nameof(CheckOut);
        public const string HotelPropertyName = nameof(Hotel);

        private DateTime _CheckIn = new DateTime();
        private DateTime _CheckOut = new DateTime();
        private Hotel _Hotel = null;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the CheckIn property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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

        /// <summary>
        /// Sets and gets the CheckOut property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
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

        /// <summary>
        /// Sets and gets the Hotel property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public virtual Hotel Hotel
        {
            get
            {
                return _Hotel;
            }

            set
            {
                if (_Hotel == value)
                {
                    return;
                }

                _Hotel = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}