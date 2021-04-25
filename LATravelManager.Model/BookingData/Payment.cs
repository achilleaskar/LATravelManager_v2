using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;
using EnumsNET;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;

namespace LATravelManager.Model.BookingData
{
    public class Payment : BaseModel
    {
        #region Constructors

        public Payment()
        {
        }

        public Payment(Payment p)
        {
            Amount = p.Amount;
            Comment = p.Comment;
            Date = p.Date;
            PaymentMethod = p.PaymentMethod;
            User = p.User;
        }

        #endregion Constructors

        #region Fields

        private decimal _Amount;
        private Booking _Booking;
        private BulkPayment _BulkPayment;

        private bool? _Checked;

        private string _Comment = string.Empty;

        private DateTime _Date;

        private bool _Outgoing;

        private ReservationWrapper _Parent;

        private PaymentMethod _PaymentMethod;

        private SolidColorBrush _PColor;

        private bool _Selected;

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

                if (Math.Abs(value - _Amount) > EPSILON)
                {
                    _Amount = Math.Round(value, 2);
                    //  AmountString = value.ToString();
                }
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

        public BulkPayment BulkPayment
        {
            get
            {
                return _BulkPayment;
            }

            set
            {
                if (_BulkPayment == value)
                {
                    return;
                }

                _BulkPayment = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBulk => BulkPayment != null;

        public bool? Checked
        {
            get
            {
                return _Checked;
            }

            set
            {
                if (_Checked == value)
                {
                    return;
                }

                _Checked = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the Comment property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [StringLength(20)]
        public string Comment
        {
            get
            {
                return _Comment;
            }

            set
            {
                if (_Comment == value)
                {
                    return;
                }

                _Comment = value;
                RaisePropertyChanged();
            }
        }

        //        if (!string.IsNullOrEmpty(_AmountString))
        //        {
        //            if (decimal.TryParse(value.Replace(',', '.'), NumberStyles.Any, new CultureInfo("en-US"), out decimal tmpDouble))
        //            {
        //                tmpDouble = Math.Round(tmpDouble, 2);
        //                Amount = tmpDouble;
        //                RaisePropertyChanged(AmountStringPropertyName);
        //                if (_AmountString[_AmountString.Length - 1] != '.' && _AmountString[_AmountString.Length - 1] != ',')
        //                {
        //                    _AmountString = tmpDouble.ToString();
        //                }
        //            }
        //            else
        //            {
        //                _AmountString = Amount.ToString();
        //            }
        //        }
        //        else
        //        {
        //            _AmountString = "0";
        //            Amount = 0;
        //        }
        //        _AmountString = value;
        //        RaisePropertyChanged(AmountStringPropertyName);
        //    }
        //}
        /// <summary>
        /// Sets and gets the Date property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
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

        public string Dates
        {
            get
            {
                if (Personal_Booking != null)
                {
                    return Personal_Booking.Dates;
                }
                else if (ThirdParty_Booking != null)
                {
                    return ThirdParty_Booking.Dates;
                }
                else if (Booking != null && Booking.Excursion != null)
                {
                    return Booking.Dates;
                }
                return "Error";
            }
        }

        [NotMapped]
        public decimal EPSILON { get; private set; } = 0.001m;

        public string ExcursionName
        {
            get
            {
                if (Personal_Booking != null)
                {
                    return Personal_Booking.Destination;
                }
                else if (ThirdParty_Booking != null)
                {
                    return ThirdParty_Booking.City;
                }
                else if (Booking != null && Booking.Excursion != null)
                {
                    return Booking.Excursion.Name;
                }
                return "Error";
            }
        }

        public string Names
        {
            get
            {
                if (Personal_Booking != null)
                {
                    return Personal_Booking.Names;
                }
                else if (ThirdParty_Booking != null)
                {
                    return ThirdParty_Booking.Names;
                }
                else if (Booking != null && Booking.Excursion != null)
                {
                    return Booking.Names;
                }
                return "Error";
            }
        }

        public bool Outgoing
        {
            get
            {
                return _Outgoing;
            }

            set
            {
                if (_Outgoing == value)
                {
                    return;
                }

                _Outgoing = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public ReservationWrapper Parent
        {
            get
            {
                return _Parent;
            }

            set
            {
                if (_Parent == value)
                {
                    return;
                }

                _Parent = value;
                RaisePropertyChanged();
            }
        }

        public string PartnerName => GetPartnerName();

        /// <summary>
        /// Sets and gets the PaymentMethod property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public PaymentMethod PaymentMethod
        {
            get
            {
                return _PaymentMethod;
            }

            set
            {
                if (_PaymentMethod == value)
                {
                    return;
                }

                _PaymentMethod = value;
                RaisePropertyChanged();
            }
        }

        [NotMapped]
        public SolidColorBrush PColor
        {
            get
            {
                if (_PColor == null)
                {
                    SetPColor();
                }
                return _PColor;
            }

            set
            {
                if (_PColor == value)
                {
                    return;
                }

                _PColor = value;
                RaisePropertyChanged();
            }
        }

        public Personal_Booking Personal_Booking { get; set; }

        [NotMapped]
        public bool Selected
        {
            get
            {
                return _Selected;
            }

            set
            {
                if (_Selected == value)
                {
                    return;
                }

                _Selected = value;
                RaisePropertyChanged();
            }
        }

        public ThirdParty_Booking ThirdParty_Booking { get; set; }

        /// <summary>
        /// Sets and gets the User property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
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

        #region Methods

        //        _AmountString = value;
        public string GetPaymentMethod()
        {
            return PaymentMethod.AsString(EnumFormat.Description);
        }

        public void SetPColor()
        {
            if (Checked == true)
            {
                PColor = new SolidColorBrush(Colors.Green);
            }
            else if (Outgoing)
            {
                PColor = new SolidColorBrush(Colors.Blue);
            }
            else if (Checked == null)
            {
                PColor = new SolidColorBrush(Colors.Transparent);
            }
            else
                PColor = new SolidColorBrush(Colors.Red);
        }

        // public const string AmountStringPropertyName = nameof(AmountString);
        /// <summary>
        /// Sets and gets the PriceString property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        //[NotMapped]
        //public string AmountString
        //{
        //    get
        //    {
        //        return _AmountString;
        //    }

        //    set
        //    {
        //        if (_AmountString == value)
        //        {
        //            return;
        //        }
        public override string ToString()
        {
            return PaymentMethod.AsString(EnumFormat.Description);
        }

        private string GetPartnerName()
        {
            if (Booking != null)
            {
                if (Booking.Partner != null)
                {
                    return Booking.Partner.Name;
                }
                return "";
            }
            if (Personal_Booking != null)
            {
                if (Personal_Booking != null)
                {
                    return Personal_Booking.Partner.Name;
                }
                return "";
            }
            if (ThirdParty_Booking != null)
            {
                if (ThirdParty_Booking.Partner != null)
                {
                    return ThirdParty_Booking.Partner.Name;
                }
                return "";
            }
            return "";
        }

        #endregion Methods
    }
}